using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端.HandleClass
{
    public class HandleWaveData
    {
        private static int CHANNEL_DATA_FILE_HEAD_LENTH = 4;//通道采样数据文件头信息长度
        private static int CHANNEL_DATA_GROUP_1_LENGTH = 0x07b0;//通道采样数据组通道1长度
        private static int CHANNEL_DATA_GROUP_2_LENGTH = 0x06ba;//通道采样数据组通道2长度
        #region MyRegion
        //录播文件二进制转换
        public WaveData binaryFileSerialize(byte[] waveByte)
        {
            WaveData wd = new WaveData();
            wd.FILE_VERSION = waveByte[0];
            wd.GPS_MARK = waveByte[1];
            wd.RESERVED_WORDS = new byte[] { waveByte[2], waveByte[3], waveByte[4], waveByte[5] };
            wd.TRIGGERING_CAUSES = new byte[18];
            for (int i = 0; i < 18; i++)
            {

                wd.TRIGGERING_CAUSES[i] = waveByte[6 + i];
            }



            int pointDataGroupsMoving = 0;
            List<object> pointDataGroups = new List<object>();
            while (pointDataGroupsMoving < waveByte.Length - 24)
            {
                pointDataGroups.Add(waveByte.Skip(24 + pointDataGroupsMoving).Take(CHANNEL_DATA_FILE_HEAD_LENTH * 2 + CHANNEL_DATA_GROUP_1_LENGTH + CHANNEL_DATA_GROUP_2_LENGTH).ToArray());
                pointDataGroupsMoving += CHANNEL_DATA_FILE_HEAD_LENTH * 2 + CHANNEL_DATA_GROUP_1_LENGTH + CHANNEL_DATA_GROUP_2_LENGTH;
            }

            wd.CHANNEL_DATA = binaryFormatChannelData(pointDataGroups);

            return wd;
        }

        //录播文件十六进制转换
        public WaveData hexadecimalFileSerialize(WaveData data)
        {
            for (int i = 0; i < data.CHANNEL_DATA.Count; i++)
            {
                ChannelDataGroup gt = data.CHANNEL_DATA[i];
                for (int j = 0; j < gt.channelDatas.Count; j++)
                {
                    ChannelData cd = gt.channelDatas[j];
                    int chanelDataMoving = 0;
                    List<int> hexadecimalFormatData = new List<int>();
                    while (chanelDataMoving < cd.POINT_DATA.Length)
                    {
                        byte[] needFormatData = cd.POINT_DATA.Skip(chanelDataMoving).Take(2).ToArray();

                        short s = 0;   //一个16位整形变量，初值为 0000 0000 0000 0000

                        s = (short)(s ^ needFormatData[1]);  //将b1赋给s的低8位
                        s = (short)(s << 8);  //s的低8位移动到高8位
                        s = (short)(s ^ needFormatData[0]);
                        hexadecimalFormatData.Add(s);
                        chanelDataMoving += 2;
                    }

                    gt.channelDatas[j].HEX_POINT_DATA = hexadecimalFormatData.ToArray();
                    gt.channelDatas[j].HEX_POINT_DATA_LENGTH = cd.HEX_POINT_DATA.Length;

                }

            }

            return data;
        }

        //录播文件通道数据二进制解压缩
        private List<ChannelDataGroup> binaryFormatChannelData(List<object> pointDataGroups)
        {
            List<ChannelDataGroup> cgl = new List<ChannelDataGroup>();
            foreach (object item in pointDataGroups)
            {
                ChannelDataGroup cg = new ChannelDataGroup();
                int channel_data_moving = 0;
                byte[] channelData = (byte[])item;
                cg.channelDatas = new List<ChannelData>();
                for (int i = 0; i <= 1; i++)
                {


                    ChannelData cd = new ChannelData();
                    cd.DATA_WIDTH = channelData[0 + channel_data_moving];
                    cd.COMPRESS_MARK = channelData[1 + channel_data_moving];
                    cd.DATA_LENGTH = (i == 0) ? CHANNEL_DATA_GROUP_1_LENGTH : CHANNEL_DATA_GROUP_2_LENGTH;
                    byte[] POINT_DATA = new byte[cd.DATA_LENGTH];
                    POINT_DATA = channelData.Skip(4 + channel_data_moving).Take(cd.DATA_LENGTH).ToArray();
                    if (cd.DATA_WIDTH == 0xff)
                    {
                        cd.POINT_DATA = POINT_DATA;
                        cd.DATA_LENGTH_BYTE = new byte[] { Convert.ToByte(cd.DATA_LENGTH & 0xff), Convert.ToByte(cd.DATA_LENGTH >> 8) };
                    }
                    else
                    {
                        List<byte> formatedDataList = new List<byte>();
                        int pointDataMoving = 0;
                        while (pointDataMoving < cd.DATA_LENGTH)
                        {

                            byte[] needFormatData = POINT_DATA.Skip(pointDataMoving).Take(3).ToArray();
                            byte[] formatedData = binaryFormatPointData(needFormatData);
                            formatedDataList = formatedDataList.Concat(formatedData.ToList()).ToList();
                            pointDataMoving += 3;
                        }


                        cd.POINT_DATA = formatedDataList.ToArray();

                        cd.DATA_LENGTH = formatedDataList.Count();

                        cd.DATA_LENGTH_BYTE = new byte[] { Convert.ToByte(cd.DATA_LENGTH & 0xff), Convert.ToByte(cd.DATA_LENGTH >> 8) };
                    }
                    cg.channelDatas.Add(cd);

                    channel_data_moving += CHANNEL_DATA_GROUP_1_LENGTH + 4;
                }

                cgl.Add(cg);
            }

            return cgl;
        }

        //反序列化录播信息
        public byte[] waveDateUnSerialize(WaveData data)
        {

            List<byte> byteList = new List<byte>();
            byteList.Add(data.FILE_VERSION);
            byteList.Add(data.GPS_MARK);
            byteList = byteList.Concat(data.RESERVED_WORDS.ToList()).ToList();
            byteList = byteList.Concat(data.TRIGGERING_CAUSES.ToList()).ToList();
            foreach (ChannelDataGroup cdg in data.CHANNEL_DATA)
            {
                foreach (ChannelData cd in cdg.channelDatas)
                {
                    byteList.Add(cd.DATA_WIDTH);
                    byteList.Add(cd.COMPRESS_MARK);
                    byteList = byteList.Concat(cd.DATA_LENGTH_BYTE.ToList()).ToList();
                    byteList = byteList.Concat(cd.POINT_DATA.ToList()).ToList();
                }
            }
            return byteList.ToArray();
        }

        private byte[] binaryFormatPointData(byte[] needFormatData)
        {

            byte c1 = needFormatData[0];
            byte c2 = Convert.ToByte(needFormatData[1] >> 4);
            int t = (needFormatData[1] << 4) & 0xff | (needFormatData[2] >> 4);
            byte c3 = Convert.ToByte((needFormatData[1] << 4) & 0xff | (needFormatData[2] >> 4));
            byte c4 = Convert.ToByte(needFormatData[2] & 0x0f);

            if ((c2 & 0x08) == 0x08)
            {
                c2 = Convert.ToByte(c2 | 0xf0);
            }

            if ((c4 & 0x08) == 0x08)
            {

                c4 = Convert.ToByte(c4 | 0xf0);
            }

            byte[] c = new byte[] { c1, c2, c3, c4 };

            return c;
        }
        #endregion
        /// <summary>
        /// 零序图表文件：第一步
        /// </summary>
        public static int[] SumCurrent(WaveData listPassagewayWave)
        {
            List<int> sumCurrent = new List<int>();
            int[] theSumCurrent = new int[listPassagewayWave.CHANNEL_DATA[0].channelDatas[0].HEX_POINT_DATA.Length];
            for (int i = 0; i < listPassagewayWave.CHANNEL_DATA.Count; i++)
            {

                for (int j = 0; j < listPassagewayWave.CHANNEL_DATA[i].channelDatas[0].HEX_POINT_DATA.Length; j++)
                {
                    theSumCurrent[j] += listPassagewayWave.CHANNEL_DATA[i].channelDatas[0].HEX_POINT_DATA[j];
                }
            }

            return theSumCurrent;
        }
        public static byte[] WriteZero(int[] theSumCurrent)
        { 
            List<byte> sc = new List<byte>();
            try
            {
                sc.Add(Convert.ToByte(0x10 & 0xff));
                sc.Add(Convert.ToByte(0x10 >> 8 & 0xff));
                sc.Add(Convert.ToByte(0x00 & 0xff));
                sc.Add(Convert.ToByte(0x00 >> 8 & 0xff));
                for (int i = 0; i < theSumCurrent.Length; i++)
                {

                    sc.Add(Convert.ToByte(theSumCurrent[i] & 0xff));
                    sc.Add(Convert.ToByte(theSumCurrent[i] >> 8 & 0xff));
                }

                string theLength = sc.Count.ToString("X2").PadLeft(4, '0');
                sc[2] = Convert.ToByte(ByteWithString.strToToHexByte(theLength)[1]);
                sc[3] = Convert.ToByte(ByteWithString.strToToHexByte(theLength)[0]);
               
            }
            catch (Exception msg)
            {
                throw msg;
            }
            return sc.ToArray();
        }
        public static string  WriteCFG(string filePath)
        {
            string showText = "";
            filePath = filePath.Replace(".dat", ".cfg");
            if (File.Exists(filePath))
            {
                //cfg
                using (StreamReader sr = new StreamReader(filePath))
                {
                    showText = Encoding.ASCII.GetString(ByteWithString.strToToHexByte(sr.ReadToEnd()));
                    showText = showText.Replace("\0", "").Replace(" ", "");
                    string sunString = Regex.Match(showText, @"01,IA1(.*?)P").Groups[1].Value;
                    string newShowText = Regex.Match(showText, @"06(.*?)P").Value;
                    string reNewShowText = newShowText + "\r\n" + "07,IZ1" + sunString + "P";
                    showText = showText.Replace(newShowText, reNewShowText);
                    sr.Close();
                }
            }
            return showText;
        }
        //.dat文件数据【相位，频率，有效值】
        public cycleDataArray SaveDataToXml(WaveData listPassagewayWave, int[] sumCurrent)
        {
            cycleDataArray theCycleDataArray = new cycleDataArray();
            theCycleDataArray.cycleDataListI = new List<List<CycleData>>();
            theCycleDataArray.cycleDataListU = new List<List<CycleData>>();
            theCycleDataArray.cycleDataListZero = new List<List<CycleData>>();
            for (int i = 0; i < listPassagewayWave.CHANNEL_DATA.Count; i++)
            {
                List<int> checkListI = listPassagewayWave.CHANNEL_DATA[i].channelDatas[0].HEX_POINT_DATA.ToList();//电流
                List<int> checkListU = listPassagewayWave.CHANNEL_DATA[i].channelDatas[1].HEX_POINT_DATA.ToList();//电压
                List<CycleData> cdI = CycleDataArray(checkListI, checkListI.Count(), 0);
                List<CycleData> cdU = CycleDataArray(checkListU, checkListU.Count(), 0);


                theCycleDataArray.cycleDataListU.Add(cdU);
                theCycleDataArray.cycleDataListI.Add(cdI);
            }
            List<int> checkListZero = sumCurrent.ToList();
            List<CycleData> cdZero = CycleDataArray(checkListZero, checkListZero.Count(), 0);
            theCycleDataArray.cycleDataListZero.Add(cdZero);
            return theCycleDataArray;
        }
        private List<CycleData> CycleDataArray(List<int> checkList, int count, int pointNum)
        {
            List<CycleData> cd = new List<CycleData>();
            while (pointNum < count)
            {
                double squareSum = 0;
                List<int> checkingList = checkList.Skip(0).Take(82).ToList();
                CycleData cdi = new CycleData();
                cdi.maxValue = MaxMinIndex.GetMaxAndIndex(checkingList.ToArray());
                cdi.minValue = MaxMinIndex.GetMinAndIndex(checkingList.ToArray());
                cdi.absValue = PLMethod(Math.Abs(cdi.maxValue - cdi.minValue));
                cdi.overZeroPoint = 0;
                for (int i = 0; i < 82; i++)
                {
                    // Math.Pow(checkingList[i], 2);
                    squareSum += Math.Pow(checkingList[i], 2);

                }
                for (int i = 0; i < 82; i++)
                {
                    if (i < 81 && cdi.overZeroPoint == 0)
                    {

                        if (checkingList[i] < 0 && checkingList[i + 1] >= 0)
                        {
                            cdi.overZeroPoint = Math.Round(((i + 1) * 360 * 1.0 / 82), 1);
                            break;
                        }
                        if (checkingList[i] > 0 && checkingList[i + 1] <= 0)
                        {
                            cdi.overZeroPoint = Math.Round(((i + 1) * 360 * 1.0 / 82), 1) + 180;
                            break;
                        }
                    }
                }
                cdi.rootValue = ((long)Math.Round(Math.Sqrt(squareSum / 82), 1) * 27778) >> 12;
                cd.Add(cdi);
                checkList.RemoveRange(0, 82);
                pointNum += 82;
            }
            return cd;
        }
        private int PLMethod(int p)
        {
            int pl = 0;
            if (p >= 30)
            {
                pl = 50;
            }
            else if (p >= 17)
            {
                pl = 100;
            }
            else if (p >= 12)
            {
                pl = 150;
            }
            else if (p >= 7)
            {
                pl = 200;
            }
            else if (p >= 5)
            {
                pl = 300;
            }
            else if (p >= 4)
            {
                pl = 400;
            }
            else
            {
                pl = 500;
            }
            return pl;
        }
    }
}
