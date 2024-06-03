using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Construct;

public static class ModAsset
{
	private static AssetRepository _repo;
	static ModAsset()
	{
		_repo = ModLoader.GetMod("Construct").Assets;
	}
	public const string ConnectionOverlayPath = @"ConnectionOverlay";
	public static Asset<Texture2D> ConnectionOverlay => _repo.Request<Texture2D>(ConnectionOverlayPath, AssetRequestMode.ImmediateLoad);
	public const string BearingPath = @"Contraptions/Anchors/Bearing";
	public static Asset<Texture2D> Bearing => _repo.Request<Texture2D>(BearingPath, AssetRequestMode.ImmediateLoad);
	public const string Bearing_MountPath = @"Contraptions/Anchors/Bearing_Mount";
	public static Asset<Texture2D> Bearing_Mount => _repo.Request<Texture2D>(Bearing_MountPath, AssetRequestMode.ImmediateLoad);
	public const string RebarItemPath = @"Contraptions/Connectors/RebarItem";
	public static Asset<Texture2D> RebarItem => _repo.Request<Texture2D>(RebarItemPath, AssetRequestMode.ImmediateLoad);
	public const string RebarTilePath = @"Contraptions/Connectors/RebarTile";
	public static Asset<Texture2D> RebarTile => _repo.Request<Texture2D>(RebarTilePath, AssetRequestMode.ImmediateLoad);
	public const string RivetedWallPath = @"Contraptions/Connectors/RivetedWall";
	public static Asset<Texture2D> RivetedWall => _repo.Request<Texture2D>(RivetedWallPath, AssetRequestMode.ImmediateLoad);
	public const string RivetedWallItemPath = @"Contraptions/Connectors/RivetedWallItem";
	public static Asset<Texture2D> RivetedWallItem => _repo.Request<Texture2D>(RivetedWallItemPath, AssetRequestMode.ImmediateLoad);
	public const string SmoothStonePath = @"Contraptions/Connectors/SmoothStone";
	public static Asset<Texture2D> SmoothStone => _repo.Request<Texture2D>(SmoothStonePath, AssetRequestMode.ImmediateLoad);
	public const string HingePath = @"Contraptions/Hinge";
	public static Asset<Texture2D> Hinge => _repo.Request<Texture2D>(HingePath, AssetRequestMode.ImmediateLoad);
	public const string Hinge_MountPath = @"Contraptions/Hinge_Mount";
	public static Asset<Texture2D> Hinge_Mount => _repo.Request<Texture2D>(Hinge_MountPath, AssetRequestMode.ImmediateLoad);
	public const string OriginPath = @"Contraptions/Origin";
	public static Asset<Texture2D> Origin => _repo.Request<Texture2D>(OriginPath, AssetRequestMode.ImmediateLoad);
	public const string Pulley_BorealPath = @"Power/Components/Pulley_Boreal";
	public static Asset<Texture2D> Pulley_Boreal => _repo.Request<Texture2D>(Pulley_BorealPath, AssetRequestMode.ImmediateLoad);
	public const string Pulley_ConnectorsPath = @"Power/Components/Pulley_Connectors";
	public static Asset<Texture2D> Pulley_Connectors => _repo.Request<Texture2D>(Pulley_ConnectorsPath, AssetRequestMode.ImmediateLoad);
	public const string Pulley_StonePath = @"Power/Components/Pulley_Stone";
	public static Asset<Texture2D> Pulley_Stone => _repo.Request<Texture2D>(Pulley_StonePath, AssetRequestMode.ImmediateLoad);
	public const string Pulley_WoodenPath = @"Power/Components/Pulley_Wooden";
	public static Asset<Texture2D> Pulley_Wooden => _repo.Request<Texture2D>(Pulley_WoodenPath, AssetRequestMode.ImmediateLoad);
	public const string InfinityGeneratorPath = @"Power/Generators/InfinityGenerator";
	public static Asset<Texture2D> InfinityGenerator => _repo.Request<Texture2D>(InfinityGeneratorPath, AssetRequestMode.ImmediateLoad);
	public const string InfinityGeneratorItemPath = @"Power/Generators/InfinityGeneratorItem";
	public static Asset<Texture2D> InfinityGeneratorItem => _repo.Request<Texture2D>(InfinityGeneratorItemPath, AssetRequestMode.ImmediateLoad);
	public const string PulleyItemPath = @"Power/PulleyItem";
	public static Asset<Texture2D> PulleyItem => _repo.Request<Texture2D>(PulleyItemPath, AssetRequestMode.ImmediateLoad);
	public const string AxlePath = @"Power/Transfer/Axle";
	public static Asset<Texture2D> Axle => _repo.Request<Texture2D>(AxlePath, AssetRequestMode.ImmediateLoad);
	public const string AxleItemPath = @"Power/Transfer/AxleItem";
	public static Asset<Texture2D> AxleItem => _repo.Request<Texture2D>(AxleItemPath, AssetRequestMode.ImmediateLoad);
	public const string SpatialBubblePath = @"SpatialBubble";
	public static Asset<Texture2D> SpatialBubble => _repo.Request<Texture2D>(SpatialBubblePath, AssetRequestMode.ImmediateLoad);
	public const string en_US_Mods_ConstructPath = @"Localization/en-US_Mods.Construct.hjson";
	public const string iconPath = @"icon";
	public static Asset<Texture2D> icon => _repo.Request<Texture2D>(iconPath, AssetRequestMode.ImmediateLoad);

}