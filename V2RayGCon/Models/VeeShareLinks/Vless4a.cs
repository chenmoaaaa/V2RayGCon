﻿using System;

namespace V2RayGCon.Models.VeeShareLinks
{
    public sealed class Vless4a :
        BasicSettings
    {
        // ver 0a is optimized for vmess protocol 
        const string version = @"4a";
        const string proto = "vless";

        public static bool IsDecoderFor(string ver) => version == ver;

        static public bool IsEncoderFor(string protocol) => protocol == proto;

        public Guid uuid;
        public string encryption;
        public string flow;

        public Vless4a() : base()
        {
            uuid = new Guid(); // zeros  
            encryption = "none";
            flow = string.Empty;
        }

        public Vless4a(BasicSettings source) : this()
        {
            CopyFrom(source);
        }

        #region public methods
        public override void CopyFromVeeConfig(Models.Datas.VeeConfigs vc)
        {
            base.CopyFromVeeConfig(vc);
            uuid = Guid.Parse(vc.auth1);
            flow = vc.auth2;
        }

        public override Datas.VeeConfigs ToVeeConfigs()
        {
            var vc = base.ToVeeConfigs();
            vc.proto = proto;
            vc.auth1 = uuid.ToString();
            vc.auth2 = flow;
            return vc;
        }

        public Vless4a(byte[] bytes) :
            this()
        {
            var ver = VgcApis.Libs.Streams.BitStream.ReadVersion(bytes);
            if (ver != version)
            {
                throw new NotSupportedException($"Not supported version ${ver}");
            }

            using (var bs = new VgcApis.Libs.Streams.BitStream(bytes))
            {
                var readString = Utils.GenReadStringHelper(bs, strTable);

                alias = bs.Read<string>();
                description = readString();
                tlsType = bs.Read<string>();
                isSecTls = bs.Read<bool>();
                port = bs.Read<int>();
                encryption = bs.Read<string>();
                uuid = bs.Read<Guid>();
                flow = bs.Read<string>();
                address = bs.ReadAddress();
                streamType = readString();
                streamParam1 = readString();
                streamParam2 = readString();
                streamParam3 = readString();
            }

            if (string.IsNullOrEmpty(encryption))
            {
                encryption = "none";
            }
        }

        public byte[] ToBytes()
        {
            byte[] result;
            using (var bs = new VgcApis.Libs.Streams.BitStream())
            {
                bs.Clear();

                var writeString = Utils.GenWriteStringHelper(bs, strTable);

                bs.Write(alias);
                writeString(description);
                bs.Write(tlsType);
                bs.Write(isSecTls);
                bs.Write(port);
                bs.Write(encryption);
                bs.Write(uuid);
                bs.Write(flow);
                bs.WriteAddress(address);
                writeString(streamType);
                writeString(streamParam1);
                writeString(streamParam2);
                writeString(streamParam3);

                result = bs.ToBytes(version);
            }

            return result;
        }

        public bool EqTo(Vless4a veeLink)
        {
            if (!EqTo(veeLink as BasicSettings)
                || encryption != veeLink.encryption
                || uuid != veeLink.uuid
                || flow != veeLink.flow)
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
