using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcDemo.Models;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace MvcDemo.Repository
{
    public class EmployeeRepository
    {
        private EmployeeModels empModels = new EmployeeModels();

        #region Select
        /// <summary>
        /// 查詢員工清單
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public List<EmployeeModels> EmpQueryByAdoNet(EmployeeViewModel viewModel)
        {
            List<EmployeeModels> empList = new List<EmployeeModels>();
            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["NorthwndConnection"].ToString();
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    String strSQL = @"
SELECT [EmployeeID]
    ,[LastName]
    ,[FirstName]
    ,[Title]
    ,[TitleOfCourtesy]
    ,[BirthDate]
    ,[HireDate]
    ,[Address]
    ,[City]
    ,[Region]
    ,[PostalCode]
    ,[Country]
    ,[HomePhone]
    ,[Extension]
    ,[Notes]
    ,[ReportsTo]
FROM Employees {0}";
                    conn.Open();

                    using (SqlCommand cmd = EmpQueryCommand(strSQL, viewModel))
                    {
                        cmd.Connection = conn;
                        SqlDataReader myDataReader = cmd.ExecuteReader();
                        while(myDataReader.Read())
                        { 
                            EmployeeModels models = new EmployeeModels();
                            models = ConvertReaderToModel<EmployeeModels>(myDataReader, models);
                            empList.Add(models);
                        }
                    }
                }

                return empList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T ConvertReaderToModel<T>(SqlDataReader reader, T model)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                PropertyInfo property = model.GetType().GetProperty(reader.GetName(i));

                property.SetValue(model, (reader.IsDBNull(i)) ? null : reader.GetValue(i));
            }
            return model;
        }

        /// <summary>
        /// 依條件產生SQL語法與回傳Command
        /// </summary>
        /// <returns></returns>
        public SqlCommand EmpQueryCommand(string Sql, EmployeeViewModel ViewModel)
        {
            List<SqlParameter> Parameter = new List<SqlParameter>();
            List<string> Condition = new List<string>();

            if (!string.IsNullOrEmpty(ViewModel.EmpId))
            {
                Condition.Add(@"EmployeeID = @EmpId");
                Parameter.Add(new SqlParameter() { ParameterName = "EmpId", SqlDbType = SqlDbType.Int, Size = 20, Value = ViewModel.EmpId });
            }

            if (!string.IsNullOrEmpty(ViewModel.EmpNm))
            {
                Condition.Add(@"LastName like @EmpNm or FirstName like @EmpNm");
                Parameter.Add(new SqlParameter() { ParameterName = "EmpNm", SqlDbType = SqlDbType.NVarChar, Size = 20, Value = "%" + ViewModel.EmpNm + "%" });
            }

            if (!string.IsNullOrEmpty(ViewModel.EmpPhone))
            {
                Condition.Add(@"HomePhone like @EmpPhone");
                Parameter.Add(new SqlParameter() { ParameterName = "EmpPhone", SqlDbType = SqlDbType.NVarChar, Size = 48, Value = "%" + ViewModel.EmpPhone + "%" });
            }

            //轉為Where條件
            string strCondition = "";
            if (Condition.Count != 0)
            {
                strCondition = " Where " + string.Join(Environment.NewLine + " AND ", Condition);
            }
            Sql = string.Format(Sql, strCondition);

            SqlCommand cmd = new SqlCommand(Sql);
            cmd.Parameters.AddRange(Parameter.ToArray());
            return cmd;
        }
        #endregion

        #region Insert
        public void InsertEmpData(EmployeeModels Model ) {
            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["NorthwndConnection"].ToString();
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string Sql = @"Insert into Employees (LastName, FirstName, BirthDate, Address) 
                                   values( @LastName, @FirstName, @BirthDate, @Address)";
                    using (SqlCommand cmd = new SqlCommand(Sql, conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("LastName", SqlDbType.NVarChar, 40).Value = Model.LastName;
                        cmd.Parameters.Add("FirstName", SqlDbType.NVarChar, 20).Value = Model.FirstName;
                        cmd.Parameters.Add("BirthDate", SqlDbType.DateTime).Value = Model.BirthDate == null ? (object)DBNull.Value : Convert.ToDateTime(Model.BirthDate).ToString("yyyy-MM-dd");
                        cmd.Parameters.Add("Address", SqlDbType.NVarChar, 120).Value = Model.Address;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //單筆入檔，就不放Transaction了
                //Write Log..
                throw ex;
            }
        }
        #endregion


        #region Update
        public void updateEmpData(EmployeeModels Model)
        {
            try
            {
                string strConn = ConfigurationManager.ConnectionStrings["NorthwndConnection"].ToString();
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    string Sql = @"UPDATE Employees
                                     SET [LastName] = @LastName
                                        ,[FirstName] = @FirstName
                                        ,[BirthDate] = @BirthDate
                                        ,[Address] = @Address
                                   WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand cmd = new SqlCommand(Sql, conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add("EmployeeID", SqlDbType.Int, 4).Value = Model.EmployeeID;
                        cmd.Parameters.Add("LastName", SqlDbType.NVarChar, 40).Value = Model.LastName;
                        cmd.Parameters.Add("FirstName", SqlDbType.NVarChar, 20).Value = Model.FirstName;
                        cmd.Parameters.Add("BirthDate", SqlDbType.DateTime).Value = Model.BirthDate == null ? (object)DBNull.Value : Convert.ToDateTime(Model.BirthDate).ToString("yyyy-MM-dd");
                        cmd.Parameters.Add("Address", SqlDbType.NVarChar, 120).Value = Model.Address;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //單筆入檔，就不放Transaction了
                //Write Log..
                throw ex;
            }
        }
        #endregion
    }
}