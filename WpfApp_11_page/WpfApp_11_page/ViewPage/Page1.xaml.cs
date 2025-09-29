﻿using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace WpfApp_11_page.ViewPage
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        private CancellationTokenSource _cancellationTokenSource;

        // 1在类级别添加字典
        private Dictionary<string, ProgressBar> _progressBars = new Dictionary<string, ProgressBar>();
        public Page1()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 添加示例图片
            AddImage("https://www.7-zip.org/7ziplogo.png",1);
            AddImage("https://picsum.photos/300/200?random=1",2);
            AddImage("https://picsum.photos/300/200?random=2",3);
            AddImage("https://picsum.photos/300/200?random=3",4);
            AddImage("https://picsum.photos/300/200?random=4",5);
        }

        private void AddImage(string imageUrl,int index)
        {
            // 创建边框容器
            Border border = new Border//设置边框
            {
                Margin = new Thickness(5),//厚度为5
                Background = Brushes.White, //背景颜色为白色
                CornerRadius = new CornerRadius(5), //圆角为5
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 10,    //模糊半径为10,设置过大会很卡
                    Color = Colors.Pink, //阴影颜色
                    Opacity = 0.3,      //透明度为0.3
                    ShadowDepth = 2     //阴影深度为2
                }
            };
            ProgressBar progressBar = new ProgressBar()
            {
                Name = "ProgressBar" + index,
                Tag = "ProgressBar" + index,
                Margin = new Thickness(5),
                Height = 5,
                Value = 0
            };
            //progressBar.Visibility = Visibility.Hidden;

            // 2添加到字典
            _progressBars["ProgressBar" + index] = progressBar;

            // 创建网格布局
            Grid grid = new Grid()
            {
                Name="Grad"+index,
                RowDefinitions =
                {
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) },//1: 比例系数（权重）GridUnitType.Star: 单位类型，表示比例分配
                    new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };

            // 创建图片
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(imageUrl)),//设置图片源
                Stretch = Stretch.UniformToFill,//图片填充方式 均匀填充
                Style = this.TryFindResource("HoverImageStyle") as Style//应用悬停样式
            };

            // 创建遮罩层
            Border overlay = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)), // 设置阴影的背景色
                Opacity = 0,                                                    // 初始完全透明（不可见）
                VerticalAlignment = VerticalAlignment.Bottom,                   // 垂直对齐到底部
                Height = 0                                                      // 初始高度为0
            };

            // 创建按钮容器
            StackPanel buttonPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            // 创建下载按钮
            Button downloadButton = new Button
            {
                Content = "下载"+"-"+index,
                Style = this.TryFindResource("ImageButtonStyle") as Style,  // 尝试从资源中查找并应用名为"ImageButtonStyle"的样式
                Tag = imageUrl                                              // 将imageUrl存储在按钮的Tag属性中（通常用于存储关联数据）
            };
            downloadButton.Click += DownloadButton_Click;                   // 为按钮的Click事件添加事件处理程序

            // 创建打开按钮
            Button openButton = new Button
            {
                Content = "打开"+"-"+index,
                Style = this.TryFindResource("ImageButtonStyle") as Style,
                Tag = imageUrl
            };
            openButton.Click += OpenButton_Click;

            // 添加按钮到面板
            buttonPanel.Children.Add(downloadButton);
            buttonPanel.Children.Add(openButton);

            // 将按钮面板添加到遮罩层
            overlay.Child = buttonPanel;

            // 添加到网格
            Grid.SetRow(progressBar,0);
            grid.Children.Add(progressBar);

            Grid.SetRow(image,1);
            Grid.SetRow(overlay,1);
            grid.Children.Add(image);
            grid.Children.Add(overlay);

            // 添加到边框
            border.Child = grid;

            // 添加鼠标事件  += 是 C# 中的事件订阅操作符，用于将方法或事件处理程序附加到事件上。
            border.MouseEnter += (s, e) => ShowButtons(overlay);
            border.MouseLeave += (s, e) => HideButtons(overlay);

            // 添加到WrapPanel
            ImageContainer.Children.Add(border);
        }

        private void ShowButtons(Border overlay)
        {
            // 创建动画显示按钮创建一个高度动画：从当前高度动画变化到100像素动画时长300毫秒
            DoubleAnimation heightAnimation = new DoubleAnimation(100, new Duration(TimeSpan.FromMilliseconds(300)));
            //创建一个透明度动画：从当前透明度动画变化到完全不透明(1) 动画时长300毫秒
            DoubleAnimation opacityAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(300)));

            //将高度动画应用到Border控件的Height属性
            overlay.BeginAnimation(Border.HeightProperty, heightAnimation);
            overlay.BeginAnimation(Border.OpacityProperty, opacityAnimation);
        }

        private void HideButtons(Border overlay)
        {
            // 创建动画隐藏按钮
            DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(300)));
            DoubleAnimation opacityAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(300)));

            overlay.BeginAnimation(Border.HeightProperty, heightAnimation);
            overlay.BeginAnimation(Border.OpacityProperty, opacityAnimation);
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string imageUrl = button.Tag as string;
            //MessageBox.Show($"开始下载图片: {imageUrl}", "下载", MessageBoxButton.OK, MessageBoxImage.Information);

            //OpenFolderDialog dialog = new();//打开文件夹

            var dialog = new SaveFileDialog();
            dialog.Filter = "可执行文件|*.exe|所有文件|*.*";
            if (dialog.ShowDialog() == true)
            {
                //dialog.Multiselect = false;//允许多选   false
                dialog.Title = "Select a folder";//标题

                string fullPathToFolder = dialog.FileName;//可以下载到这个路径下

                // 初始化取消令牌
                _cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _cancellationTokenSource.Token;

                // 更新UI状态
                button.IsEnabled = false;
                //找到进度条控件的名字


                // 通过名称查找控件
                //ProgressBar progressBar = ImageContainer.Children
                //.OfType<ProgressBar>()
                //.FirstOrDefault(pb => pb.Tag?.ToString() == "ProgressBar" + button.Content.ToString().Split("-")[1]);

                // 3在DownloadButton_Click中直接使用
                string index = button.Content.ToString().Split("-")[1];
                ProgressBar progressBar = _progressBars["ProgressBar" + index];

                if (progressBar != null)
                {
                    progressBar.Value = 0;
                }
                button.Content = "开始下载...";

                try
                {
                    await DownloadFileAsync("https://www.7-zip.org/a/7z2501-x64.exe", progressBar, fullPathToFolder, cancellationToken);
                    MessageBox.Show("下载完成！");
                }
                catch (OperationCanceledException)//取消操作
                {
                    MessageBox.Show("下载已取消,需手动删除已下载的部分文件才能继续下载");
                    // 删除部分下载的文件
                    if (File.Exists(fullPathToFolder))
                    {
                        File.Delete(fullPathToFolder);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("下载失败");
                }
                finally
                {
                    // 恢复UI状态
                    button.IsEnabled = true;
                    _cancellationTokenSource?.Dispose();//确保释放取消令牌
                    _cancellationTokenSource = null;
                }

            }

        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string imageUrl = button.Tag as string;


            // 打开默认浏览器并导航到指定URL
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.baidu.com",
                UseShellExecute = true
            });
        }


        private async Task DownloadFileAsync(string url,ProgressBar progressBar, string filePath, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                // 设置超时时间
                httpClient.Timeout = TimeSpan.FromMinutes(30);

                //得到的响应数据
                using (var response = await httpClient.GetAsync(
                    url, 
                    HttpCompletionOption.ResponseHeadersRead, 
                    cancellationToken))
                {
                    response.EnsureSuccessStatusCode();// 确保响应成功

                    var totalBytes = response.Content.Headers.ContentLength ?? -1L;//获取文件总大小，如果未知则为-1
                    var canReportProgress = totalBytes != -1; //是否可以报告进度（只有在知道文件大小时才能计算百分比）

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                        //FileMode.Create: 创建新文件，如果存在则覆盖
                        //FileAccess.Write: 只写访问
                        //FileShare.None: 不允许其他进程访问
                        //8192: 缓冲区大小
                        //true: 使用异步I / O
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        var buffer = new byte[8192];// 8KB缓冲区
                        var totalBytesRead = 0L; // 总共已读取的字节数
                        var bytesRead = 0;      // 每次读取的字节数

                        //从buffer数组的第0个位置开始存放数据,最多读取buffer.Length大小（装满整个水桶）
                        //Stream流会自动保留当前读取的位置，下次读取的时候会从上次读取的位置开始读取
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                        {
                            //从读取的buffer数组的第0个位置开始来写入
                            //bytesRead: 实际要写入的字节数,就是读取的字节数
                            await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                            totalBytesRead += bytesRead;//累计计数: 记录总共读取的字节数

                            if (canReportProgress)//判断是否可以报告进度
                            {
                                var progressPercentage = (double)totalBytesRead / totalBytes * 100;//计算进度百分比
                                // 通过名称查找控件

                                if (progressBar != null)
                                {
                                    progressBar.Value = progressPercentage;
                                }
                                
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }

        //private async void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Button? button = sender as Button;
        //    _cancellationTokenSource?.Cancel();
        //    button.Content = "正在取消下载...";
        //}
    }
}
