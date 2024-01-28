using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp
{
    public class Banking
    {
        static SqlConnection Connect() { 
        SqlConnection conn = null;
        string str = ConfigurationManager.ConnectionStrings["ConStr"].ToString();   
        conn = new SqlConnection(str);
        return conn;
        }

        public void AddAccount()
        {
            try {
                using (SqlConnection c = Connect()) { 
                c.Open();
                    Console.WriteLine("Enter FirstName :");
                    string firstName=Console.ReadLine();
                    Console.WriteLine("Enter Last Name :");
                    string lastName=Console.ReadLine();
                    Console.WriteLine("Enter Address :");
                    string address=Console.ReadLine();
                    Console.WriteLine("Enter City");
                    string city=Console.ReadLine();
                    Console.WriteLine("Enter Email :");
                    string Email=Console.ReadLine();
                    Console.WriteLine("Enter Mobile Number :");
                    string phone=Console.ReadLine();
                    Console.WriteLine("Enter Account Type : ");
                    string AccType=Console.ReadLine();
                    Console.WriteLine("Enter Branch Id :");
                    int branchId=Convert.ToInt32(Console.ReadLine());
                    SqlCommand insertPer = new SqlCommand("insert into PersonalDetails(FirstName,LastName,Address,City,EmailId,MobileNum) values(@FirstName, @LastName, @Address, @City, @Email, @MobileNum);SELECT SCOPE_IDENTITY()", c);
        
                    insertPer.Parameters.AddWithValue("@FirstName", firstName);
                    insertPer.Parameters.AddWithValue("@LastName", lastName);
                    insertPer.Parameters.AddWithValue("@Address", address);
                    insertPer.Parameters.AddWithValue("@City", city);
                    insertPer.Parameters.AddWithValue("@Email", Email);
                    insertPer.Parameters.AddWithValue("@MobileNum", phone);
                    insertPer.ExecuteNonQuery();

                    //Get PersonId and AccountId  
                    SqlCommand command = new SqlCommand("Select PersonId, AccountId from PersonalDetails where EmailId=@Email", c);

                    command.Parameters.AddWithValue("@Email", Email);

                    SqlDataReader reader = command.ExecuteReader();
                    int personalId = 0, accountId = 0;
                    while (reader.Read())
                    {
                         personalId = (int)reader["PersonId"];
                         accountId = (int)reader["AccountId"];
                    }
                    c.Close();
                    //insert into AccountDetails Table
                    SqlCommand insertAcc = new SqlCommand("insert into AccountDetails values(@AccountId,@PersonId,@AccountType,@ClosingBalance,@BranchId)", c);
                    c.Open();
                    float cb = 0;
                    insertAcc.Parameters.AddWithValue("@AccountId", accountId);
                    insertAcc.Parameters.AddWithValue("@PersonId", personalId);
                    insertAcc.Parameters.AddWithValue("@AccountType", AccType);
                    insertAcc.Parameters.AddWithValue("@ClosingBalance", cb);
                    insertAcc.Parameters.AddWithValue("@BranchId", branchId);
                    insertAcc.ExecuteNonQuery();
                    Console.WriteLine("Account Created Successfully with Account Id : {0}",accountId);
                }
            } 
            catch (Exception e) { Console.WriteLine(e); }
        }
        public void Deposit()
        {
            try {
                using (SqlConnection c = Connect()) {
                    c.Open();
                    Console.WriteLine("Enter Account Id to Deposit Amount");
                    int aid=int.Parse(Console.ReadLine());
                    SqlCommand command = new SqlCommand($"select ClosingBalance from AccountDetails where AccountId={aid}", c);
                    SqlDataReader reader = command.ExecuteReader();
                    float cb =0;
                    if (reader.HasRows) { 
                        while (reader.Read())
                    {
                        cb = Convert.ToSingle(reader["ClosingBalance"]);
                    }
                    c.Close();
                    c.Open ();
                    Console.WriteLine("Enter Amount to Deposit :");
                    float dpa=float.Parse(Console.ReadLine());
                    float tol = cb + dpa;
                    SqlCommand updateCb = new SqlCommand($"update AccountDetails SET ClosingBalance = @bal WHERE AccountId = @Accid ",c);
                    updateCb.Parameters.AddWithValue("@Accid", aid);
                    updateCb.Parameters.AddWithValue("@bal",tol);
                    updateCb.ExecuteNonQuery();
                    Console.WriteLine("Your Closing Balance is : {0}",tol);
                }
                    else { Console.WriteLine("Account Id not Valid"); }
            }
            }catch (Exception e) { Console.WriteLine(e); }
        }
        public void Withdral()
        {
            try
            {
                using (SqlConnection c = Connect())
                {
                    c.Open();
                    Console.WriteLine("Enter Account Id to Withdral Amount");
                    int aid = int.Parse(Console.ReadLine());
                    SqlCommand command = new SqlCommand($"select ClosingBalance from AccountDetails where AccountId={aid}", c);
                    SqlDataReader reader = command.ExecuteReader();
                    float cb = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cb = Convert.ToSingle(reader["ClosingBalance"]);
                        }
                   
                    c.Close();
                    c.Open();
                    Console.WriteLine("Enter Amount to Withdral :");
                    float dpa = float.Parse(Console.ReadLine());
                    float tol = cb - dpa;
                    SqlCommand updateCb = new SqlCommand($"update AccountDetails SET ClosingBalance = @bal WHERE AccountId = @Accid ", c);
                    updateCb.Parameters.AddWithValue("@Accid", aid);
                    updateCb.Parameters.AddWithValue("@bal", tol);
                    updateCb.ExecuteNonQuery();
                    Console.WriteLine("Your Closing Balance is : {0}", tol);
                    }
                    else { Console.WriteLine("Account Id not Valid"); }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
    }
}
