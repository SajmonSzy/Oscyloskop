/**
  ******************************************************************************
  * @file    LCD_AnimatedPicture\LCD_AnimatedPicture\Program.cs
  * @author  MCD
  * @version V1.0.0
  * @date    24-Sep-2013
  * @brief   Main program body
  ******************************************************************************
   * @attention
  *
  * <h2><center>&copy; COPYRIGHT 2013 STMicroelectronics</center></h2>
  *
  * Licensed under MCD-ST Liberty SW License Agreement V2, (the "License");
  * You may not use this file except in compliance with the License.
  * You may obtain a copy of the License at:
  *
  *        http://www.st.com/software_license_agreement_liberty_v2
  *
  * Unless required by applicable law or agreed to in writing, software 
  * distributed under the License is distributed on an "AS IS" BASIS, 
  * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
  * See the License for the specific language governing permissions and
  * limitations under the License.
  *
  * <h2><center>&copy; COPYRIGHT 2013 STMicroelectronics</center></h2>
  */

/* References ------------------------------------------------------------------*/
using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Hardware;
using STM32F429I_Discovery.Netmf.Hardware;

namespace Microsoft.SPOT.Sample.LCD
{
    public class LCD
    {
        static AnalogOutput DAC_CH0, DAC_CH1;

        public static void Main()
        {
            Bitmap bitmap1 = new Bitmap(SystemMetrics.ScreenWidth, SystemMetrics.ScreenHeight);
            int maxsamples = 300;
            int[] samples;
            samples = new int[maxsamples];

            /* Delay between each image display*/
            int Delayms = 10;
            
            Bitmap Img = Resources.GetBitmap(Resources.BitmapResources.Image01);
            bitmap1.DrawImage(0, 0, Img, 0, 0, Img.Width, Img.Height);
            bitmap1.Flush();
            Thread.Sleep(2000);
            /* Calculate the starting X and Y coordinates for images*/
           // int X_Start = (SystemMetrics.ScreenWidth - Img.Width) / 2;
            //int Y_Start = (SystemMetrics.ScreenHeight - Img.Height) / 2;
            AnalogInput ADC0 = new AnalogInput(ADC.Channel0_PA6);
            DAC_CH0 = new AnalogOutput(DAC.Channel1_PA5);
            double ain0;
            double ss,scaley;
            int xs,ys,xi;
            /* Display 22 images in loop with a predefined delay between them */
            int ii = 0;
            scaley = 100/(2*2*2*2*2*2*2*2*2*2*2*2.0);
            ss = 1;
            xs = 10;
            ys = 120;
            xi = 0;
            long ts,te,ts2,te2;
            double o1, o2, o3;
            o3 = 0;
            while (true)
            {

                //bitmap1.Clear();
                //Img = Resources.GetBitmap(Resources.BitmapResources.Image01);
               // bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                //bitmap1.DrawRectangle(Color.Black, 0, 0, 0, 200, 50, 0, 0, Color.Black, 0, 0, 0, 0, 0, 0);
                ts = DateTime.UtcNow.Ticks;
                for (int k = 0; k < maxsamples; k++)
                {
                    samples[k] = ADC0.ReadRaw();
                    if (k < (maxsamples - 3))
                        o1 = samples[k] + samples[k + 1];
                    else
                        o1 = samples[k];

                    o2 = o1 * 3 / 4.0 * 15.4;
                    o3 += (2.4*o1 + o2 * 3.1)/100.0;
                    
                    DAC_CH0.Write((Math.Sin(k)+1000)/2000.0);
                }
                te = DateTime.UtcNow.Ticks;
                bitmap1.Clear();
                string txt;
                //ain0=ADC0.Read();
                //txt =  ain0.ToString();
                
           //     ts2 = DateTime.UtcNow.Ticks;
             //   Thread.Sleep(100);
              //  te2 = DateTime.UtcNow.Ticks;

                txt = ((te - ts)/1000).ToString()+" ms "+o3.ToString();// +" " + (te2 - ts2).ToString();
                string txt2;

                txt2 = (SystemMetrics.ScreenHeight).ToString() + "x"+(SystemMetrics.ScreenWidth).ToString();
                Font myFont = Resources.GetFont(Resources.FontResources.small); 
              //  bitmap1.DrawText( txt, myFont, Colors.Blue,10,10);
                bitmap1.DrawTextInRect(txt, 10, 10, 200, 50, 0, Colors.Blue, myFont);
                bitmap1.DrawTextInRect(txt2, 10, 200, 200, 50, 0, Colors.Blue, myFont);

                
                
                
                
               // bitmap1.DrawLine(Colors.Red, 1, (int)(xs), (int)(ys), (int)(xs + ss * (60 + 1)), (int)(ys));
                //bitmap1.DrawLine(Colors.Red, 1, (int)(xs), (int)(ys), (int)(xs), (int)(ys - scaley));

                for (xi = 0; xi < maxsamples; xi++)
                {
                    ain0 = samples[xi];
                    int x1, y1,x2,y2;
                    //x-y zamienione
                    y1=(int)(xs + ss * xi);
                    x1=(int)(ys + scaley * ain0);
                    y2 = (int)(xs + ss * (xi + 1));
                    x2 = (int)(ys + scaley * ain0);
                    bitmap1.DrawLine(Colors.Cyan, 1,x1 ,y1 , x2, y2);
                }
                bitmap1.DrawText(ii.ToString(), myFont, Colors.Blue, 10, 40);
                bitmap1.Flush();

                
               // Thread.Sleep(Delayms);
            /*    Img = Resources.GetBitmap(Resources.BitmapResources.Image02);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image03);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image04);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image05);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image06);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image07);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image08);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image09);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image10);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image11);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image12);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image13);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image14);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image15);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image16);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image17);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image18);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image19);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image20);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image21);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);
                Img = Resources.GetBitmap(Resources.BitmapResources.Image22);
                bitmap1.DrawImage(X_Start, Y_Start, Img, 0, 0, Img.Width, Img.Height);
                bitmap1.Flush();
                Thread.Sleep(Delayms);*/
            }
        }
    }
}
/******************* (C) COPYRIGHT STMicroelectronics *****END OF FILE****/