using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows;
using System.Collections.ObjectModel;

namespace Barber.Code
{
    class bookingtime
    {
        public TimeSpan time { get; set; }
        public DateTime date { get; set; }

        public static ObservableCollection<bookingtime> bookingtimepush(int emp_id, DateTime dates)
        {
            ObservableCollection<bookingtime> bookingtimes = new ObservableCollection<bookingtime>();
            string d = dates.Year.ToString() + "-" + dates.Month.ToString() + "-" + dates.Day.ToString();
            string strcon = ConfigurationManager.ConnectionStrings["defcon2"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                string cmd = "select times.val from times where dbo.free_emp_time("+ emp_id.ToString() + ",'" + d + "',times.val) != 0";
                SqlCommand sqlCommand = new SqlCommand(cmd, con);
                SqlDataReader data = sqlCommand.ExecuteReader();
                while (data.Read())
                {
                    bookingtime btime = new bookingtime();
                    btime.time = data.GetTimeSpan(0);
                    btime.date = dates;
                   
                    bookingtimes.Add(btime);
                }
                con.Close();
            }
            return bookingtimes;
        }

    }
}
