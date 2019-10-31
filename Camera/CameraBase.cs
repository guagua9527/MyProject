﻿using CameraSDK.Enum;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraSDK.Camera
{
    /// <summary>
    /// 相机基类
    /// </summary>
    public abstract class CameraBase : IDisposable
    {
        public readonly string IpAddress;     //相机IP地址
        public readonly short Port;             //相机端口
        public readonly string UserName;      //用户名
        public readonly string Password;      //密码

        public string SavePath;

        /// <summary>
        /// 云台控制成员
        /// </summary>
        public PTZControlBase PTZControl;

        /// <summary>
        /// 播放控件句柄
        /// </summary>
        internal IntPtr PlayControlHandle;

        /// <summary>
        /// 播放句柄
        /// </summary>
        internal IntPtr PlayHandle;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlay { get; protected set; } = false;

        /// <summary>
        /// 画面回调委托
        /// </summary>
        /// <param name="mat">当前帧图像</param>
        public delegate void VideoDataCallBackHanlder(Mat mat);
        public event VideoDataCallBackHanlder VideoDataCallBackEvent;

        public CameraBase(string ip, short port, string userName, string password)
        {
            this.IpAddress = ip;
            this.Port = port;
            this.UserName = userName;
            this.Password = password;
        }

        public CameraBase(CAMERA_CONFIG config):this(config.Ip, config.Port, config.UserName, config.Password)
        {
            if (config.HaveNullOrEmpty())
            {
                throw new Exception("Config 字段不能为空!");
            }
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        public abstract bool Init();

        /// <summary>
        /// 注册画面回调事件
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public abstract bool RegistVideoDataCallBack(VideoDataCallBackHanlder callback);

        /// <summary>
        /// 登录相机
        /// </summary>
        protected abstract bool Camera_Login(string ipAddress, int port, string userName, string password);

        /// <summary>
        /// 退出登录
        /// </summary>
        protected abstract bool Camera_Logout();

        /// <summary>
        /// 初始化SDK
        /// </summary>
        protected abstract bool SDK_Init();

        /// <summary>
        /// 释放SDK
        /// </summary>
        protected abstract bool SDK_Dispose();

        public abstract bool SaveRealData();
        public abstract bool StopSaveRealData();

        /// <summary>
        /// 启动实时预览
        /// </summary>
        /// <param name="hwnd">播放控件句柄</param>
        public abstract bool StartRealPlay(IntPtr hwnd);

        /// <summary>
        /// 停止实时预览
        /// </summary>
        public abstract bool StopRealPlay();

        /// <summary>
        /// 获取最新的SDK错误码
        /// </summary>
        /// <returns></returns>
        public abstract int GetLastError();


        //TODO  云台操作

        /// <summary>
        /// 释放相机资源
        /// </summary>
        public abstract void Dispose();
    }

    public struct CAMERA_CONFIG
    {
        public string Ip;
        public short Port;

        public string UserName;
        public string Password;

        public CAMERA_CONFIG(string ip, short port, string username, string password)
        {
            this.Ip = ip;
            this.Port = port;
            this.UserName = username;
            this.Password = password;
        }

        public bool HaveNullOrEmpty()
        {
            return string.IsNullOrEmpty(Ip)
                && string.IsNullOrEmpty(UserName)
                && string.IsNullOrEmpty(Password)
                && Port == 0;
        }

        public override string ToString()
        {
            return $"Ip:{Ip}, Port:{Port}, UserName:{UserName}, Password:{Password}";
        }
    }
}