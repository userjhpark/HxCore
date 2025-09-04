using System.Data;

namespace HxCore
{
    public interface IHxBase : System.IDisposable
    {
        //string Name { get; }

        //void Dispose();
        string GetName();
    }

    public interface IHxSetValue
    {
        void SetValue(DataRow row);
        void SetValue(DataTable data, int rowIndex = 0);
    }

    public interface IHxSetValueRecord<T> : IHxSetValue
        where T : struct
    {
        string COL_CUSTOM_USER_AGENT { get; }
        //void SetValue(DataRow row);
        //void SetValue(DataTable dt, int index = 0);
        void SetValue(DataView dv, int index = 0);
        void SetMatchFieldValue(string name, object value);
        string GetCustomUserAgentString(IHxDb db);
        void CopyData(T value);
        void Clear();

    }
}