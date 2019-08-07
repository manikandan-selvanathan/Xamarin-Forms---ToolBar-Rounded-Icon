using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RoundedToolBarItem.Droid;
using Android.Graphics.Drawables;
using Plugin.CurrentActivity;
using static Android.Graphics.Bitmap;
using Android.Graphics;

[assembly: ExportRenderer(typeof(ContentPage), typeof(DroidContentPageRenderer))]
namespace RoundedToolBarItem.Droid
{
    public class DroidContentPageRenderer : PageRenderer
    {
        private Context context;
        public DroidContentPageRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null || Element == null)
            {
                if (Element == null)
                    return;

                if (context == null) context = CrossCurrentActivity.Current.AppContext;

                var toolbar = (context as MainActivity).FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                if (toolbar != null)
                {
                    var menu = toolbar.Menu;
                    if (menu != null && menu.HasVisibleItems)
                    {
                        int i = 0;
                        foreach (var toolbarItem in Element.ToolbarItems)
                        {
                            try
                            {
                                if (toolbarItem is RoundToolbarItem)
                                {
                                    var nativeToolbarItem = menu.GetItem(Element.ToolbarItems.IndexOf(toolbarItem));
                                    var bitmap = ((BitmapDrawable)nativeToolbarItem.Icon).Bitmap;

                                    var minDimention = Math.Min(bitmap.Width, bitmap.Height);
                                    var roundBitMap = DroidImageExtention.RoundImage(bitmap, minDimention / 2);

                                    nativeToolbarItem.SetIcon(new BitmapDrawable(Resources, roundBitMap));
                                }
                                i++;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
        }
    }

    public static class DroidImageExtention
    {
        public static Bitmap RoundImage(Bitmap bitmap, int cornerRadius)
        {
            Bitmap output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Config.Argb8888);
            Canvas canvas = new Canvas(output);
            Paint paint = new Paint();
            Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
            RectF rectF = new RectF(rect);
            float roundPx = cornerRadius;
            paint.AntiAlias = true;
            canvas.DrawARGB(0, 0, 0, 0);
            canvas.DrawRoundRect(rectF, roundPx, roundPx, paint);

            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.DrawBitmap(bitmap, rect, rect, paint);

            return output;
        }
    }
}