﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HsmBI.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute(@"metadata=res://*/HSEntity.csdl|res://*/HSEntity.ssdl|res://*/HSEntity.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=SQL5008.Smarterasp.net;initial catalog=DB_98E871_hsmdb_admin;integrated security=True;id=DB_98E871_hsmdb_admin;Password=hsm00912;MultipleActiveResultSets=True;App=EntityFramework&quot;")]
        public string defaultcon {
            get {
                return ((string)(this["defaultcon"]));
            }
        }
    }
}
