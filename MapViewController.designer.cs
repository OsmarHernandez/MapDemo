// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MapDemo
{
    [Register ("MapViewController")]
    partial class MapViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton downButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton leftButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView map { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton rightButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton upButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (downButton != null) {
                downButton.Dispose ();
                downButton = null;
            }

            if (leftButton != null) {
                leftButton.Dispose ();
                leftButton = null;
            }

            if (map != null) {
                map.Dispose ();
                map = null;
            }

            if (rightButton != null) {
                rightButton.Dispose ();
                rightButton = null;
            }

            if (upButton != null) {
                upButton.Dispose ();
                upButton = null;
            }
        }
    }
}