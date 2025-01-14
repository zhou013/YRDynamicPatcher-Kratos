
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DynamicPatcher;
using PatcherYRpp;
using Extension.Ext;
using Extension.Script;

namespace ExtensionHooks
{
    public class BulletExtHooks
    {
        [Hook(HookType.AresHook, Address = 0x4664BA, Size = 5)]
        public static unsafe UInt32 BulletClass_CTOR(REGISTERS* R)
        {
            return BulletExt.BulletClass_CTOR(R);
        }

        [Hook(HookType.AresHook, Address = 0x4665E9, Size = 0xA)]
        public static unsafe UInt32 BulletClass_DTOR(REGISTERS* R)
        {
            return BulletExt.BulletClass_DTOR(R);
        }

        [Hook(HookType.AresHook, Address = 0x46AFB0, Size = 8)]
        [Hook(HookType.AresHook, Address = 0x46AE70, Size = 5)]
        public static unsafe UInt32 BulletClass_SaveLoad_Prefix(REGISTERS* R)
        {
            return BulletExt.BulletClass_SaveLoad_Prefix(R);
        }

        [Hook(HookType.AresHook, Address = 0x46AF97, Size = 7)]
        [Hook(HookType.AresHook, Address = 0x46AF9E, Size = 7)]
        public static unsafe UInt32 BulletClass_Load_Suffix(REGISTERS* R)
        {
            return BulletExt.BulletClass_Load_Suffix(R);
        }

        [Hook(HookType.AresHook, Address = 0x46AFC4, Size = 5)]
        public static unsafe UInt32 BulletClass_Save_Suffix(REGISTERS* R)
        {
            return BulletExt.BulletClass_Save_Suffix(R);
        }

        [Hook(HookType.AresHook, Address = 0x466556, Size = 6)]
        public static unsafe UInt32 BulletClass_Init(REGISTERS* R)
        {
            try
            {
                Pointer<BulletClass> pBullet = (IntPtr)R->ECX;
                BulletExt ext = BulletExt.ExtMap.Find(pBullet);
                // Logger.Log("BulletExt init {0}", ext == null?"Ext is null":"is ready.");
                ext?.OnInit();
                ext.Scriptable?.OnInit();
            }
            catch (Exception e)
            {
                Logger.PrintException(e);
            }
            return (uint)0;
        }

        [Hook(HookType.AresHook, Address = 0x468B5D, Size = 6)]
        public static unsafe UInt32 BulletClass_Put(REGISTERS* R)
        {
            try
            {
                Pointer<BulletClass> pBullet = (IntPtr)R->EBX;
                BulletExt ext = BulletExt.ExtMap.Find(pBullet);
                Pointer<CoordStruct> pCoord = R->Stack<IntPtr>(-0x20);
                // Logger.Log("BulletExt init {0} {1}", ext == null?"Ext is null":"is ready.", pCoord.Data);
                ext?.OnPut(pCoord);
                ext.Scriptable?.OnPut(pCoord, default);
            }
            catch (Exception e)
            {
                Logger.PrintException(e);
            }
            return (uint)0;
        }

        [Hook(HookType.AresHook, Address = 0x4666F7, Size = 6)]
        public static unsafe UInt32 BulletClass_Update(REGISTERS* R)
        {
            try
            {
                Pointer<BulletClass> pBullet = (IntPtr)R->EBP;
                BulletExt ext = BulletExt.ExtMap.Find(pBullet);
                ext?.OnUpdate();
                ext.Scriptable?.OnUpdate();
            }
            catch (Exception e)
            {
                Logger.PrintException(e);
            }
            return (uint)0;
        }

    }
}