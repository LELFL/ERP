namespace ELF.Common.Models;
public class AmisGetListInputBase
{
    public int? _page;
    public int? Page { get => _page is null || _page == 0 ? 1 : _page; set => _page = value; }

    public int? _perPage;
    public int? PerPage { get => _perPage is null ? 0 : _perPage; set => _perPage = value; }

    public int? Skip => (Page - 1) * PerPage;
    public int? Take => PerPage;


    public string? Sorting
    {
        get
        {
            if (string.IsNullOrEmpty(OrderBy)) return null;
            if (string.IsNullOrEmpty(OrderDir)) OrderDir = "asc";
            return OrderBy + " " + OrderDir;
        }
        set { }
    }
    public string? OrderBy { get; set; }
    public string? OrderDir { get; set; }
}
