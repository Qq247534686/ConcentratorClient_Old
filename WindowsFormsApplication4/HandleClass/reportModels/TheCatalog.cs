using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    /// <summary>
    /// 目录类
    /// </summary>
    public class TheCatalog
    {
        /// <summary>
        /// 目录名称
        /// </summary>
        public byte[] CATALOG_NAME { get; set; }
        /// <summary>
        /// 设备地址
        /// </summary>
        public byte[] DEVICE_ADDRESS { get; set; }
        /// <summary>
        /// 文件信息
        /// </summary>
        public List<CatalogFiles> THE_CATALOG_FILES { get; set; }
        /// <summary>
        /// 文件数量
        /// </summary>
        public byte FILES_COUNT { get; set; }
    }
    public class CatalogFiles
    {
        /// <summary>
        /// 文件名长度
        /// </summary>
        public byte FILE_LENGTH{ get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public byte[] FILE_NAME { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public byte[] DeviceAddress { get; set; }
        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] FILE_CONTENT { get; set; }
    }
}
