using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class WaveData
    {
        public byte FILE_VERSION { get; set; }//文件版本字
        public byte GPS_MARK { get; set; }//GPS/北斗锁定标记
        public byte[] RESERVED_WORDS { get;set;}//保留字
        public byte[] TRIGGERING_CAUSES { get; set; }//触发原因
        public List<ChannelDataGroup> CHANNEL_DATA { get; set; }//通道数据组
    }

    public class ChannelDataGroup {

        public List<ChannelData> channelDatas{get;set;}//通道数据
    }

    public class ChannelData
    {
        public byte DATA_WIDTH { get; set; }//数据位宽
        public byte COMPRESS_MARK { get; set; }//压缩标记
        public Int32 DATA_LENGTH { get; set; }//数据长度
        public byte[] DATA_LENGTH_BYTE { get; set; }
        public byte[] POINT_DATA { get; set; }//采样数据点集合,Y轴,0.24
        public Int32 HEX_POINT_DATA_LENGTH { get; set; }//数据长度
        public int[] HEX_POINT_DATA { get; set; }//采样数据点集合
    }
    public class PassagewayWave
    {
        public int PassagewayCount { get; set; }
        public List<int> FirstPassageway { get; set; }
        public List<int> SecondPassageway { get; set; }
    }
}
