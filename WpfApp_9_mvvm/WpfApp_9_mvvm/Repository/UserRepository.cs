using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp_9_mvvm.Model;

namespace WpfApp_9_mvvm.Repository
{
    public class UserRepository
    {
        // 添加用户
        public void AddUser(User user)
        {
            using (var connection = UserDatabaseHelper.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO User (Name, Email, Age,Address)
                VALUES (@Name, @Email, @Age,@Address)";

                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Age", user.Age ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", user.Address);

                //用来执行增（INSERT）、删（DELETE）、改（UPDATE）操作,属于非查询操作
                command.ExecuteNonQuery();
            }
        }

        // 获取所有用户
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            //获取数据库连接
            using (var connection = UserDatabaseHelper.GetConnection())
            {
                //打开数据库连接
                connection.Open();

                //创建 SQL 命令对象,设置要执行的 SQL 查询语句
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM User";

                //使用 ExecuteReader() 执行查询，返回 SqliteDataReader 对象
                using (var reader = command.ExecuteReader())
                {
                    //逐行读取查询结果
                    while (reader.Read())
                    {
                        //从当前行读取数据并创建 User 对象
                        users.Add(new User
                        {
                            //Id = reader.GetInt32(0),
                            //Name = reader.GetString(1),
                            //Email = reader.GetString(2),
                            //Age = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                            // 使用列名而不是索引，提高代码可读性和可维护性
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            // 将 Age 的赋值方式修正为支持可空类型的写法
                            Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Age")),
                            Address = reader["Address"].ToString()
                        });
                    }
                }
            }

            return users;
        }

        // 更新用户
        public void UpdateUser(User user)
        {
            using (var connection = UserDatabaseHelper.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                UPDATE User
                SET Name = @Name, Email = @Email, Age = @Age,Address = @Address
                WHERE Name = @Name";

                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Age", user.Age ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", user.Address);
                //用来执行增（INSERT）、删（DELETE）、改（UPDATE）操作,属于非查询操作

                command.ExecuteNonQuery();
            }
        }

        // 删除用户
        public void DeleteUser(string Name)
        {
            using (var connection = UserDatabaseHelper.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM User WHERE Name = @Name";
                command.Parameters.AddWithValue("@Name", Name);

                command.ExecuteNonQuery();
            }
        }
    }
}
