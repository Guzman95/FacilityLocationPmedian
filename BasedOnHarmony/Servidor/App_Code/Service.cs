using System.Data;
using System.Data.SqlClient;

public class Service : IService
{
    //private const string StrCon = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""F:\UNIVERSIDAD\TESIS\FacilityLocationPmedian\BasedOnHarmony\Servidor\App_Data\BDTareas.mdf"";Integrated Security=True";
    private const string StrCon = "Server=sql5105.site4now.net;Database=db_a77411_pmedian;User ID=db_a77411_pmedian_admin;Password=1234tesis;Trusted_Connection=False;MultipleActiveResultSets=true";
    /// <summary>
    /// Return only one Distributed task to be executed for one client (status N),
    /// The assigned task change its status to P for Processing, thus preventing
    /// another client from trying to execute that task.
    /// </summary>
    /// <returns></returns>
	public DistributeTask GetTask()
	{
        var myConexion = new SqlConnection(StrCon);
        var mySql = "SELECT TOP (1) Dt_Id, Dt_Problem, Dt_Algorithm, " +
                                  " Dt_Seed, Dt_Result_Best " +
                          "FROM     DistributeTask " +
                          "WHERE    Dt_Status = 'N' " +
                          "ORDER BY Dt_Id ASC";
        var myDs = new DataSet();
        var myAdapter = new SqlDataAdapter(mySql, myConexion);
        myAdapter.Fill(myDs, "DistributeTask");

        if (myDs.Tables[0].Rows.Count == 0)
            return null; //There is no Distributed Tasks to be executed

        myConexion.Open();
        var thisRow = myDs.Tables[0].Rows[0];

        var myDistributedTask = new DistributeTask
        {
            Id = thisRow["Dt_Id"].ToString(),
            Problem = thisRow["Dt_Problem"].ToString(),
            Algorithm = thisRow["Dt_Algorithm"].ToString(),
            Seed = int.Parse(thisRow["Dt_Seed"].ToString())
        };
        mySql = "UPDATE DistributeTask " +
                "SET    Dt_Status ='P' " +
                "WHERE  Dt_Id = '" + myDistributedTask.Id + "' ";
        var miComando = new SqlCommand(mySql, myConexion);
        miComando.ExecuteNonQuery();
        myConexion.Close();

		return myDistributedTask;
	}

    /// <summary>
    /// Status for a DistributedTask are Succesfull (S),
    /// No assigned (N) and Processing (P)
    /// </summary>
    /// <param name="miDistributeTask"></param>
    /// <returns></returns>
    public bool SaveResults(DistributeTask miDistributeTask)
    {
        var miConexion = new SqlConnection(StrCon);
        miConexion.Open();

        var miSql = "UPDATE DistributeTask " +
                          "SET Dt_Status = 'S', " +
                          "    Dt_Result_Best = '" + miDistributeTask.Result_Best + "', " +
                          "    Dt_Date = GetDate() " +
                          "WHERE  Dt_Id = '" + miDistributeTask.Id + "' ";
        var miComando = new SqlCommand(miSql, miConexion);
        var filasAfectadas = miComando.ExecuteNonQuery();

        miConexion.Close();
        return (filasAfectadas != 0);
    }
}