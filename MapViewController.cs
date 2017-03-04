using System;
using UIKit;
using CoreLocation;
using MapKit;
using Foundation;

namespace MapDemo
{
	partial class MapViewController : UIViewController
	{
		MyMapDelegate mapDel;
		UISearchController searchController;
		CLLocationManager locationManager = new CLLocationManager ();
		bool fuera = true;

		public MapViewController (IntPtr handle) : base (handle)
		{
			
		}

		public void check(double lon, double lat) {
			if ((lon <= ((-34 * 0.0005) - 106.48694)) && (lat >= ((15 * 0.0005) + 31.739444)) && fuera) {
				fuera = false;
	
				var notification = new UILocalNotification();

				// set the fire date (the date time in which it will fire)
				notification.FireDate = NSDate.Now.AddSeconds(1); //DateTime.Now.AddSeconds(10));
				notification.TimeZone = NSTimeZone.DefaultTimeZone;
				// configure the alert stuff
				notification.AlertTitle = "Peligro";
				notification.AlertAction = "Alert Action";
				notification.AlertBody = "¡Has entrado en una zona de peligro!";

				notification.UserInfo = NSDictionary.FromObjectAndKey(new NSString("UserInfo for notification"), new NSString("Notification"));

				notification.SoundName = UILocalNotification.DefaultSoundName;

				// schedule it
				UIApplication.SharedApplication.ScheduleLocalNotification(notification);
			}
				
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			locationManager.RequestWhenInUseAuthorization ();

			// set map type and show user location
			map.MapType = MKMapType.Standard;
			map.ShowsUserLocation = true;
			map.Bounds = View.Bounds;

			// set map center and region
			double lat = 31.739444;
			double lon = -106.48694;
			var mapCenter = new CLLocationCoordinate2D (lat, lon);
			var mapRegion = MKCoordinateRegion.FromDistance (mapCenter, 3000, 3000);
			map.CenterCoordinate = mapCenter;
			map.Region = mapRegion;

			// add an annotation
			var actualAnnotation = new MKPointAnnotation();
			actualAnnotation.Title = "Ubicacion Actual";
			actualAnnotation.Coordinate = new CLLocationCoordinate2D(lat, lon);
			map.AddAnnotation(actualAnnotation);

			// set the map delegate
			mapDel = new MyMapDelegate();
			map.Delegate = mapDel;

			/* add a custom annotation
			map.AddAnnotation (new MonkeyAnnotation ("Xamarin", mapCenter));
			*/

			// add an overlay
			var circleOverlay = MKCircle.Circle(mapCenter, 200);
			map.AddOverlay(circleOverlay);

			// Danger Zone
			var mapDanger = new CLLocationCoordinate2D(31.7526, -106.5053);
			var dangerOverlay_01 = MKCircle.Circle(mapDanger, 400);
			map.AddOverlay(dangerOverlay_01);

			// circleOverlay intersects with dangerOverlay_01


			// Busqueda
			var searchResultsController = new SearchResultsViewController (map);

			var searchUpdater = new SearchResultsUpdator ();
			searchUpdater.UpdateSearchResults += searchResultsController.Search;

			//add the search controller
			searchController = new UISearchController (searchResultsController) {
				SearchResultsUpdater = searchUpdater
			};

			searchController.SearchBar.SizeToFit ();
			searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
			searchController.SearchBar.Placeholder = "Enter a search query";

			searchController.HidesNavigationBarDuringPresentation = false;
			NavigationItem.TitleView = searchController.SearchBar;
			DefinesPresentationContext = true;

			/* Button Actions */
			upButton.TouchUpInside += (object sender, EventArgs e) => {
				map.RemoveAnnotation(actualAnnotation);
				map.RemoveOverlay(circleOverlay);

				lat = lat + 0.0005;
				actualAnnotation.Title = "Ubicacion Actual";
				actualAnnotation.Coordinate = new CLLocationCoordinate2D(lat, lon);
				map.AddAnnotation(actualAnnotation);

				mapCenter = new CLLocationCoordinate2D(lat, lon);
			 	circleOverlay = MKCircle.Circle(mapCenter, 200);
				map.AddOverlay(circleOverlay);

				check(lon, lat);
			};

			leftButton.TouchUpInside += (object sender, EventArgs e) => {
				map.RemoveAnnotation(actualAnnotation);
				map.RemoveOverlay(circleOverlay);

				lon = lon - 0.0005;
				actualAnnotation.Title = "Ubicacion Actual";
				actualAnnotation.Coordinate = new CLLocationCoordinate2D(lat, lon);
				map.AddAnnotation(actualAnnotation);

				mapCenter = new CLLocationCoordinate2D(lat, lon);
				circleOverlay = MKCircle.Circle(mapCenter, 200);
				map.AddOverlay(circleOverlay);

				check(lon, lat);
			};

			rightButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				map.RemoveAnnotation(actualAnnotation);
				map.RemoveOverlay(circleOverlay);

				lon = lon + 0.0005;
				actualAnnotation.Title = "Ubicacion Actual";
				actualAnnotation.Coordinate = new CLLocationCoordinate2D(lat, lon);
				map.AddAnnotation(actualAnnotation);

				mapCenter = new CLLocationCoordinate2D(lat, lon);
				circleOverlay = MKCircle.Circle(mapCenter, 200);
				map.AddOverlay(circleOverlay);

				check(lon, lat);
			};

			downButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				map.RemoveAnnotation(actualAnnotation);
				map.RemoveOverlay(circleOverlay);

				lat = lat - 0.0005;
				actualAnnotation.Title = "Ubicacion Actual";
				actualAnnotation.Coordinate = new CLLocationCoordinate2D(lat, lon);
				map.AddAnnotation(actualAnnotation);

				mapCenter = new CLLocationCoordinate2D(lat, lon);
				circleOverlay = MKCircle.Circle(mapCenter, 200);
				map.AddOverlay(circleOverlay);

				check(lon, lat);
			};
		}

		public class SearchResultsUpdator : UISearchResultsUpdating
		{
			public event Action<string> UpdateSearchResults = delegate {};

			public override void UpdateSearchResultsForSearchController (UISearchController searchController)
			{
				this.UpdateSearchResults (searchController.SearchBar.Text);
			}
		}

		class MyMapDelegate : MKMapViewDelegate
		{
			string pId = "PinAnnotation";
			string mId = "MonkeyAnnotation";

			public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
			{
				MKAnnotationView anView;

				if (annotation is MKUserLocation)
					return null; 

				if (annotation is MonkeyAnnotation) {

					// show monkey annotation
					anView = mapView.DequeueReusableAnnotation (mId);

					if (anView == null)
						anView = new MKAnnotationView (annotation, mId);

					anView.Image = UIImage.FromFile ("monkey.png");
					anView.CanShowCallout = true;
					anView.Draggable = true;
					anView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);

				} else {

					// show pin annotation
					anView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation (pId);

					if (anView == null)
						anView = new MKPinAnnotationView (annotation, pId);

					((MKPinAnnotationView)anView).PinColor = MKPinAnnotationColor.Red;
					anView.CanShowCallout = true;
				}

				return anView;
			}

			public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
			{
				var monkeyAn = view.Annotation as MonkeyAnnotation;

				if (monkeyAn != null) {
					var alert = new UIAlertView ("Monkey Annotation", monkeyAn.Title, null, "OK");
					alert.Show ();
				}
			}

			public override MKOverlayView GetViewForOverlay (MKMapView mapView, IMKOverlay overlay)
			{
				var circleOverlay = overlay as MKCircle;
				var circleView = new MKCircleView (circleOverlay);
				circleView.FillColor = UIColor.Red;
				circleView.Alpha = 0.4f;
				return circleView;
			}
		}
	}
}
