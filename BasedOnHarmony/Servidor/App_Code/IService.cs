using System.Runtime.Serialization;
using System.ServiceModel;

[ServiceContract]
public interface IService
{
	[OperationContract]
    DistributeTask GetTask();

    [OperationContract]
    bool SaveResults(DistributeTask miDistributeTask);
}

[DataContract]
public class DistributeTask
{
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Problem { get; set; }

    [DataMember]
    public string Algorithm { get; set; }

    [DataMember]
    public string Parameters { get; set; }

    [DataMember]
    public int Seed { get; set; }

    [DataMember]
    public string Status { get; set; }

    [DataMember]
    public double Result_Best { get; set; }

    [DataMember]
    public int Optimal { get; set; }

}