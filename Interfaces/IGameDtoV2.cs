namespace GameStore.Api.Interfaces;

public interface IGameDtoV2 : IGameDtoV1
{
	decimal DiscountedPrice { get; set; }
}
