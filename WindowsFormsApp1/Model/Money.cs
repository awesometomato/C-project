﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class Money
    {
        private String kind;
        private int won;
        private int year;
        private int month;
        private int day;
        private string income_expense;

        public String Kind
        {
            get { return kind; }
            set { kind = value; }
        }
        public int Won
        {
            get { return won; }
            set { won = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public int Month
        {
            get { return month; }
            set { month = year; }
        }
        public int Day
        {
            get { return day; }
            set { day = value; }
        }
        public string Income_expense
        {
            get { return income_expense; }
            set { income_expense = value; }
        }

        public Money()
        {
            kind = "";
            won = 0;
            year = 0;
            month = 0;
            day = 0;
            income_expense = "지출";
        }

        public Money(String kind, int won, int year, int month, int day, string income_expense)
        {
            this.kind = kind;
            this.won = won;
            this.year = year;
            this.month = month;
            this.day = day;
            this.income_expense = income_expense;
        }

        public Money(int num)
        {
            try
            {
                string selectQuery = "SELECT * FROM money WHERE num = '" + num.ToString() + "'";
                MySqlDataAdapter adpt = new MySqlDataAdapter(selectQuery, PrimaryOperation.connection);

                DataSet ds = new DataSet();
                adpt.Fill(ds, "money");

                if (ds.Tables.Count > 0)
                {
                    DataRow row;
                    row = ds.Tables[0].Rows[0];

                    Kind = row["memo"].ToString();
                    Won = int.Parse(row["money"].ToString());
                    year = int.Parse(row["date"].ToString().Substring(0,4));
                    //MessageBox.Show(row["date"].ToString());
                    month = int.Parse(row["date"].ToString().Substring(5,2));
                    day = int.Parse(row["date"].ToString().Substring(8,2));
                    income_expense = row["sign"].ToString();

                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public string AddMoney(string money, string sign, string year, string month, string day, string memo, int num)
        {
            if (money == "" || sign == "2" || year =="" || month == "" || day == "")
            {
                return "모든 항목을 입력해주세요.";
            }

            int i;
            if (int.TryParse(money, out i) == false) { return "금액은 숫자로 입력해주세요."; }

            string date = "00000000";
            if (month.Length == 1) { month = "0" + month; }
            if (day.Length == 1) { day = "0" + day; }
            date = year + month + day;

            //MySqlConnection connection = new MySqlConnection("Server=localhost;Database=project;Uid=root;Pwd=s17011564!;");

            string query;
            if (num == 0)
                query = "INSERT INTO money (id,money,sign,date,memo) VALUES('" + PrimaryOperation.currentUser.Id + "'," + money + ",'" + sign + "'," + date + ",'" + memo + "')";
            else
                query = "update money set money = " + money + ", sign = '" + sign + "', date = " + date + ", memo = '" + memo + "' where num = " + num + "";
                


            PrimaryOperation.connection.Open();
            MySqlCommand command = new MySqlCommand(query, PrimaryOperation.connection);

            try
            {
                if (command.ExecuteNonQuery() == 1)
                {
                    return "정상적으로 입력되었습니다.";
                }
                else { return "오류!!"; }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                PrimaryOperation.connection.Close();
            }
        }

        public DataSet GetDataSet(string date)
        {
            int year = int.Parse(date.Substring(0, 4));
            int month = int.Parse(date.Substring(4, 2));
            int day = int.Parse(date.Substring(6, 2));

            DataSet ds = new DataSet();
            String selectQuery = "select * from money where id = '" + PrimaryOperation.currentUser.Id + "'";
            if (year != 0)
                selectQuery += " and year(date) = " + year + "";
            if (month != 0)
                selectQuery += " and month(date) = " + month + "";
            if (day != 0)
                selectQuery += " and day(date) = " + day + "";

            MySqlDataAdapter adpt = new MySqlDataAdapter(selectQuery, PrimaryOperation.connection);
            adpt.Fill(ds, "money");

            return ds;
        }

        public string DeleteMoney(int num)
        {
            string query;

            query = "Delete from money where num = " + num + "";
            
            PrimaryOperation.connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, PrimaryOperation.connection);
            cmd.ExecuteNonQuery();
            PrimaryOperation.connection.Close();

            return "해당 항목을 삭제하였습니다.";
           
        }
    }
}
