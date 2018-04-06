using System;
using System.Data.Entity.Spatial;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui
{
  //===========================================================================================================================
  public static class SpatialUtil
  {
    public static DbGeography CreatePoint(double Latitude, double Longitude, int SRID = 4326)
    {
      string WKT = String.Format("POINT({0} {1})", Longitude, Latitude);

      return DbGeography.PointFromText(WKT, SRID);
    }
  }
}