namespace Ase.Doc.Demo.Model;

public class UploadResults
{
    public string DealerCode { get; set; }
    public string DealerName { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<UploadResultValue> Values { get; set; }
}

public class UploadResultValue
{
    public string Code { get; set; }
    public string Value { get; set; }
    public float? Confidence { get; set; }
}
