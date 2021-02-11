using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace PhoneMapper
{
    public class MapFunctionsClass
    {
        private GMapControl _gMapControl;
        private GMapOverlay _markersOverlay;
        private GMapOverlay _selectionMarkersOverlay;

        public MapFunctionsClass(GMapControl gMapControl)
        {
            _gMapControl = gMapControl;
            initMap();
        }

       
        private void initMap()
        {
            _gMapControl.Overlays.Clear();

            _gMapControl.MapProvider = OpenStreetMapProvider.Instance; //use Open Street Maps as the tile provider
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly; //don't ever load tiles from cache
            _gMapControl.Position = new GMap.NET.PointLatLng(51.477928, -0.001545); //center the map at GMT position
            _gMapControl.ShowCenter = false; //remove the red cross at the center of map

            _markersOverlay = new GMapOverlay("markerOverlay1"); //create an instance of an overlay layer for the map for placing markers
            _selectionMarkersOverlay = new GMapOverlay("markerOverlay2"); //create another instance of an overlay layer for the map for placing selected markers
            _gMapControl.Overlays.Add(_markersOverlay); //add the above overlay layer to the map
            _gMapControl.Overlays.Add(_selectionMarkersOverlay); //add the abover overlay layer to the map
        }

        public void centerMapAt(double lat, double lng)
        {
            _gMapControl.Position = new GMap.NET.PointLatLng(lat, lng);
        }

        public void putMarkerAt(int tableIndex, string dateTime, double lat, double lng, GMarkerGoogleType markerType)
        {
            GMapMarker gMapMarker = new GMarkerGoogle(new GMap.NET.PointLatLng(lat, lng), markerType);

            gMapMarker.ToolTipText = dateTime;
            gMapMarker.ToolTipMode = MarkerTooltipMode.Never;

            gMapMarker.Tag = tableIndex; //index of the row corresponding to this marker in the listview table
            _markersOverlay.Markers.Add(gMapMarker);
        }

        public void putSelectionMarkerAt(List<(string, double, double)> latLngList, GMarkerGoogleType markerType)
        {
            _selectionMarkersOverlay.Clear(); //clear all previous selection markers

            foreach ((string dateTime, double lat, double lng) in latLngList)
            {
                GMapMarker gMapMarker = new GMarkerGoogle(new GMap.NET.PointLatLng(lat, lng), markerType);

                gMapMarker.ToolTipText = dateTime;
                gMapMarker.ToolTipMode = MarkerTooltipMode.Never;

                _selectionMarkersOverlay.Markers.Add(gMapMarker);
            }

        }

        public void setTooltipMode(MarkerTooltipMode tooltipMode)
        {
            List<GMapMarker> standardMarkers = _markersOverlay.Markers.ToList();
            List<GMapMarker> selectedMarkers = _selectionMarkersOverlay.Markers.ToList();

            //update the tooltip display behavior for each marker on map(both default and the selected markers)
            foreach (GMapMarker marker in standardMarkers.Concat(selectedMarkers))
            {
                marker.ToolTipMode = tooltipMode;
            }

        }

    }
}
