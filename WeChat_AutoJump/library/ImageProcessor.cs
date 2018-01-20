using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;


namespace WeChat_AutoJump.library
{
    /// <summary>
    /// 图像处理算法类
    /// </summary>
    class ImageProcessor
    {
        /// <summary>
        /// 模板文件路径
        /// </summary>
        private string screencapPath = @"autojump.png";
        private string jumperPath = Environment.CurrentDirectory + "\\Template\\jumperTemplate.png";
        private string roundPath = Environment.CurrentDirectory + "\\Template\\roundTemplate.png";
        private string centerPath = Environment.CurrentDirectory + "\\Template\\centerTemplate.png";

        /// <summary>
        /// 图像处理流程
        /// </summary>
        /// <param name="sourcePath">源图像路径</param>
        /// <param name="tpoint">输出目标点坐标</param>
        /// <param name="distance">输出跳跃距离</param>
        public void ImageScan(string sourcePath, ref Point tpoint,ref double distance)
        {
            //输入xy坐标应随机，如一直保持同一点会被Ban。例如320+rand，410+rand
            ImageProcessor imp = new ImageProcessor();
            Image<Rgb, Byte> roiImage = imp.SetROIArea(screencapPath);
            Point[] jumperPointArray = null, centerPointArray = null;
            double[] jumperMaxArray = null, centerMaxArray = null;
            imp.MatchTemplate(roiImage, jumperPath, ref jumperPointArray, ref jumperMaxArray);
            Point jumperPoint = imp.CharacterDetect(jumperPointArray);

            #region ImageProcess

            Point targetPoint = new Point();           
            try
            {
                imp.MatchTemplate(roiImage, centerPath, ref centerPointArray, ref centerMaxArray);
                Point centerPoint = imp.CenterDetect(centerPointArray, centerMaxArray);
                if (centerPoint != new Point(0, 0))
                {
                    targetPoint = centerPoint;
                }
                else
                {
                    centerPoint = imp.SquareDetect(roiImage, jumperPoint);
                    if (centerPoint != new Point(0, 0))
                    {
                        targetPoint = centerPoint;
                    }
                    else
                    {
                        targetPoint = imp.EllipseDetect(roiImage);
                    }
                }
            }
            catch
            {
                targetPoint = new Point(0, 0);
            }

            #endregion

            MCvScalar scalar = new MCvScalar(255);
            Rectangle rect = new Rectangle(targetPoint.X, targetPoint.Y, 3, 3);
            CvInvoke.Rectangle(roiImage, rect, scalar, 5, LineType.EightConnected);

            distance = Math.Sqrt(Math.Pow(Math.Abs(targetPoint.X - jumperPoint.X), 2) +
                                 Math.Pow(Math.Abs(targetPoint.Y - jumperPoint.Y), 2)); 
            tpoint = targetPoint;
        }

        /// <summary>
        /// ROI区域设置
        /// </summary>
        /// <param name="sourcePath">源文件路径</param>
        /// <returns>设置ROI后的图像</returns>
        public Image<Rgb, Byte> SetROIArea(string sourcePath)
        {
            Image<Rgb, Byte> sourceImage = new Image<Rgb, Byte>(sourcePath);
            Rectangle roiRectangle = new Rectangle(
                0,
                Convert.ToInt32(sourceImage.Height / 3),
                sourceImage.Width,
                Convert.ToInt32(sourceImage.Height / 3));
            sourceImage.ROI = roiRectangle;
            return sourceImage;
        }

        /// <summary>
        /// 图形匹配算法
        /// </summary>
        /// <param name="sourceImage">源图像</param>
        /// <param name="tempPath">模板路径</param>
        /// <param name="point">输出左上角坐标</param>
        /// <param name="maxval">输出最大匹配值</param>
        private void  MatchTemplate(Image<Rgb, Byte> sourceImage, string tempPath,ref Point[] point, ref double[] maxval)
        {
            Image<Rgb, Byte> templateImage = new Image<Rgb, Byte>(tempPath);
            Image<Gray, float> resultImage = sourceImage.MatchTemplate(
                templateImage,
                TemplateMatchingType.CcorrNormed);

            double[] min, max;
            Point[] point1, point2;
            resultImage.MinMax(out min, out max, out point1, out point2);
            point = point2;
            maxval = max;
            
        }

        /// <summary>
        /// 游戏角色匹配
        /// </summary>
        /// <param name="point">角色左上角坐标</param>
        /// <returns>角色下方中心点坐标</returns>
        private Point CharacterDetect(Point[] point)
        {
            Point p = new Point();
            p.X = point[0].X + 47;
            p.Y = point[0].Y + 222;
            return p;
        }

        /// <summary>
        /// 中心白点匹配
        /// </summary>
        /// <param name="point">白点左上角坐标</param>
        /// <returns>白点正中坐标</returns>
        private Point CenterDetect(Point[] point, double[] maxval)
        {
            Point p = new Point();
            if (maxval[0]>0.97)
            {
                p.X = point[0].X + 21;
                p.Y = point[0].Y + 14;
                return p;
            }
            else
            {
                return p;
            }

        }

        /// <summary>
        /// 方块中心点OpenCV检测
        /// </summary>
        /// <param name="sourceImage">源图像</param>
        /// <param name="p">角色底部中点坐标</param>
        /// <returns>方块中心点坐标，如无目标方块返回(0,0)</returns>
        private Point SquareDetect(Image<Rgb, Byte> sourceImage,Point p)
        {
            Image<Gray, Byte> gray = sourceImage.Convert<Gray, Byte>().PyrDown().PyrUp();
            double cannyThreshold = 5.0;
            double cannyThreshodLinking = 5.0;
            Image<Gray, Byte> canniedEdges = gray.Canny(cannyThreshold, cannyThreshodLinking);

            #region LineDetect
            
            LineSegment2D[] lines = CvInvoke.HoughLinesP(
                canniedEdges,
                10, //Distance resolution in pixel-related units
                Math.PI / 6.0, //Angle resolution measured in radians.
                80, //threshold
                30, //min Line width
                5); //gap between lines

            Point centerpoint = new Point();

            try
            {
                LineSegment2D[] linesR = lines.Where(l => l.Length > 140 && l.Side(p) == 1).ToArray();

                LineSegment2D maxLineR1 = linesR[0], maxLineR2 = linesR[0];
                for (int i = 1; i < linesR.Length; i++)
                {
                    maxLineR1 = maxLineR1.P1.Y < linesR[i].P1.Y ? maxLineR1 : linesR[i];
                    maxLineR2 = maxLineR2.P2.Y < linesR[i].P2.Y ? maxLineR2 : linesR[i];
                }

                centerpoint.X = Convert.ToInt32((maxLineR1.P1.X + maxLineR2.P2.X) / 2);
                centerpoint.Y = Convert.ToInt32((maxLineR1.P2.Y + maxLineR2.P1.Y) / 2);
                if (centerpoint.Y > p.Y - 111)
                {
                    centerpoint.X = 0;
                    centerpoint.Y = 0;
                }
            }
            catch
            {
                centerpoint.X = 0;
                centerpoint.Y = 0;
            }

            #endregion


            return centerpoint;
        }

        /// <summary>
        /// 椭圆中心点OpenCV检测
        /// </summary>
        /// <param name="sourceImage">源图片</param>
        /// <returns>椭圆形中心点坐标,如检测不到返回（0,0）</returns>
        private Point EllipseDetect(Image<Rgb, Byte> sourceImage)
        {
            //此算法有待改善

            #region EllipseDetect

            double cannyThreshold = 5.0;
            double cannyThreshodLinking = 5.0;

            Image<Gray, Byte> tempImage = new Image<Gray, Byte>(roundPath);
            Image<Gray, Byte> gray = sourceImage.Convert<Gray, Byte>().PyrDown().PyrUp();
            Image<Gray, Byte> canniedEdges = gray.Canny(cannyThreshold, cannyThreshodLinking);
            Image<Gray, Byte> canniedTemp = tempImage.Canny(cannyThreshold, cannyThreshodLinking);

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(canniedEdges, contours, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

            VectorOfVectorOfPoint contoursTemp = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(canniedTemp, contoursTemp, null, RetrType.External, ChainApproxMethod.ChainApproxNone);

            List<Point> pointList = new List<Point>();
            Point maxPoint = new Point();
            try
            {
                for (int i = 0; i < contours.Size; i++)
                {
                    double result = CvInvoke.MatchShapes(contoursTemp[0], contours[i], ContoursMatchType.I1);
                    double area = CvInvoke.ContourArea(contours[i], false);

                    if (result < 3 && area > 120)
                    {
                        for (int j = 0; j < contours[i].Size; j++)
                        {
                            pointList.Add(contours[i][j]);
                        }
                    }
                }
                maxPoint = pointList[0];
                foreach (Point pt in pointList)
                {
                    maxPoint = maxPoint.Y < pt.Y ? maxPoint : pt;
                }
                maxPoint.Y = maxPoint.Y + 70;
            }
            catch
            {
                maxPoint.X = 0;
                maxPoint.Y = 0;
            }


            //MCvScalar scalar = new MCvScalar(255);
            //CvInvoke.DrawContours(sourceImage, contours, i, scalar, 5, LineType.EightConnected);
            //Rectangle rect = new Rectangle(maxPoint.X, maxPoint.Y+70, 3, 3);
            //CvInvoke.Rectangle(sourceImage, rect, scalar, 5, LineType.EightConnected);

            #endregion

            return maxPoint;

        }

    }
}
