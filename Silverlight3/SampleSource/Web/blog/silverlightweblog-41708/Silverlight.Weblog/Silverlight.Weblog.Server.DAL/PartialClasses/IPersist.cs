namespace Silverlight.Weblog.Server.DAL.PartialClasses
{
    public interface IPersist<T>
    {
        void Create();
        void Delete();
    }
}