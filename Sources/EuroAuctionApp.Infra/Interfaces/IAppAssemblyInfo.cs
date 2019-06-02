namespace EuroAuctionApp.Infra.Interfaces
{
    public interface IAppAssemblyInfo
    {
        string AppName { get; }

        string Product { get; }

        string Copyright { get; }

        string Version { get; }

        string Description { get; }
    }
}