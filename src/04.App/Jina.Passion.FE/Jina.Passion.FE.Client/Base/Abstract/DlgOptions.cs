namespace Jina.Passion.FE.Client.Base.Abstract
{
    public class DlgOptions : DlgOptionsBase
    {
    }

    public class DlgOptions<T> : DlgOptions
    {
        public T Param { get; set; }
    }
}