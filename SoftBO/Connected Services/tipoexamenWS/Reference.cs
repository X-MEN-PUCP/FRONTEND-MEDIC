﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoftBO.tipoexamenWS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", ConfigurationName="tipoexamenWS.TipoExamenWS")]
    public interface TipoExamenWS {
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/modificarTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/modificarTipoExamenResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftBO.tipoexamenWS.modificarTipoExamenResponse modificarTipoExamen(SoftBO.tipoexamenWS.modificarTipoExamenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/modificarTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/modificarTipoExamenResponse")]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.modificarTipoExamenResponse> modificarTipoExamenAsync(SoftBO.tipoexamenWS.modificarTipoExamenRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/listarTodosTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/listarTodosTipoExamenResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftBO.tipoexamenWS.listarTodosTipoExamenResponse listarTodosTipoExamen(SoftBO.tipoexamenWS.listarTodosTipoExamenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/listarTodosTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/listarTodosTipoExamenResponse")]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.listarTodosTipoExamenResponse> listarTodosTipoExamenAsync(SoftBO.tipoexamenWS.listarTodosTipoExamenRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/obtenerPorIdTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/obtenerPorIdTipoExamenResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftBO.tipoexamenWS.obtenerPorIdTipoExamenResponse obtenerPorIdTipoExamen(SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/obtenerPorIdTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/obtenerPorIdTipoExamenResponse")]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.obtenerPorIdTipoExamenResponse> obtenerPorIdTipoExamenAsync(SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest request);
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/insertarTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/insertarTipoExamenResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        SoftBO.tipoexamenWS.insertarTipoExamenResponse insertarTipoExamen(SoftBO.tipoexamenWS.insertarTipoExamenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/insertarTipoExamenRequest", ReplyAction="http://softcitws.soft.pucp.edu.pe/TipoExamenWS/insertarTipoExamenResponse")]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.insertarTipoExamenResponse> insertarTipoExamenAsync(SoftBO.tipoexamenWS.insertarTipoExamenRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/")]
    public partial class tipoExamenDTO : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int idTipoExamenField;
        
        private bool idTipoExamenFieldSpecified;
        
        private string indicacionField;
        
        private string nombreTipoExamenField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int idTipoExamen {
            get {
                return this.idTipoExamenField;
            }
            set {
                this.idTipoExamenField = value;
                this.RaisePropertyChanged("idTipoExamen");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idTipoExamenSpecified {
            get {
                return this.idTipoExamenFieldSpecified;
            }
            set {
                this.idTipoExamenFieldSpecified = value;
                this.RaisePropertyChanged("idTipoExamenSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string indicacion {
            get {
                return this.indicacionField;
            }
            set {
                this.indicacionField = value;
                this.RaisePropertyChanged("indicacion");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string nombreTipoExamen {
            get {
                return this.nombreTipoExamenField;
            }
            set {
                this.nombreTipoExamenField = value;
                this.RaisePropertyChanged("nombreTipoExamen");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="modificarTipoExamen", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class modificarTipoExamenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen;
        
        public modificarTipoExamenRequest() {
        }
        
        public modificarTipoExamenRequest(SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen) {
            this.tipoExamen = tipoExamen;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="modificarTipoExamenResponse", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class modificarTipoExamenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int @return;
        
        public modificarTipoExamenResponse() {
        }
        
        public modificarTipoExamenResponse(int @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="listarTodosTipoExamen", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class listarTodosTipoExamenRequest {
        
        public listarTodosTipoExamenRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="listarTodosTipoExamenResponse", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class listarTodosTipoExamenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftBO.tipoexamenWS.tipoExamenDTO[] @return;
        
        public listarTodosTipoExamenResponse() {
        }
        
        public listarTodosTipoExamenResponse(SoftBO.tipoexamenWS.tipoExamenDTO[] @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtenerPorIdTipoExamen", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class obtenerPorIdTipoExamenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int tipoExamenId;
        
        public obtenerPorIdTipoExamenRequest() {
        }
        
        public obtenerPorIdTipoExamenRequest(int tipoExamenId) {
            this.tipoExamenId = tipoExamenId;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="obtenerPorIdTipoExamenResponse", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class obtenerPorIdTipoExamenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftBO.tipoexamenWS.tipoExamenDTO @return;
        
        public obtenerPorIdTipoExamenResponse() {
        }
        
        public obtenerPorIdTipoExamenResponse(SoftBO.tipoexamenWS.tipoExamenDTO @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="insertarTipoExamen", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class insertarTipoExamenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen;
        
        public insertarTipoExamenRequest() {
        }
        
        public insertarTipoExamenRequest(SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen) {
            this.tipoExamen = tipoExamen;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="insertarTipoExamenResponse", WrapperNamespace="http://softcitws.soft.pucp.edu.pe/", IsWrapped=true)]
    public partial class insertarTipoExamenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://softcitws.soft.pucp.edu.pe/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int @return;
        
        public insertarTipoExamenResponse() {
        }
        
        public insertarTipoExamenResponse(int @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface TipoExamenWSChannel : SoftBO.tipoexamenWS.TipoExamenWS, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class TipoExamenWSClient : System.ServiceModel.ClientBase<SoftBO.tipoexamenWS.TipoExamenWS>, SoftBO.tipoexamenWS.TipoExamenWS {
        
        public TipoExamenWSClient() {
        }
        
        public TipoExamenWSClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public TipoExamenWSClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TipoExamenWSClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public TipoExamenWSClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftBO.tipoexamenWS.modificarTipoExamenResponse SoftBO.tipoexamenWS.TipoExamenWS.modificarTipoExamen(SoftBO.tipoexamenWS.modificarTipoExamenRequest request) {
            return base.Channel.modificarTipoExamen(request);
        }
        
        public int modificarTipoExamen(SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen) {
            SoftBO.tipoexamenWS.modificarTipoExamenRequest inValue = new SoftBO.tipoexamenWS.modificarTipoExamenRequest();
            inValue.tipoExamen = tipoExamen;
            SoftBO.tipoexamenWS.modificarTipoExamenResponse retVal = ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).modificarTipoExamen(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.modificarTipoExamenResponse> SoftBO.tipoexamenWS.TipoExamenWS.modificarTipoExamenAsync(SoftBO.tipoexamenWS.modificarTipoExamenRequest request) {
            return base.Channel.modificarTipoExamenAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftBO.tipoexamenWS.modificarTipoExamenResponse> modificarTipoExamenAsync(SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen) {
            SoftBO.tipoexamenWS.modificarTipoExamenRequest inValue = new SoftBO.tipoexamenWS.modificarTipoExamenRequest();
            inValue.tipoExamen = tipoExamen;
            return ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).modificarTipoExamenAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftBO.tipoexamenWS.listarTodosTipoExamenResponse SoftBO.tipoexamenWS.TipoExamenWS.listarTodosTipoExamen(SoftBO.tipoexamenWS.listarTodosTipoExamenRequest request) {
            return base.Channel.listarTodosTipoExamen(request);
        }
        
        public SoftBO.tipoexamenWS.tipoExamenDTO[] listarTodosTipoExamen() {
            SoftBO.tipoexamenWS.listarTodosTipoExamenRequest inValue = new SoftBO.tipoexamenWS.listarTodosTipoExamenRequest();
            SoftBO.tipoexamenWS.listarTodosTipoExamenResponse retVal = ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).listarTodosTipoExamen(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.listarTodosTipoExamenResponse> SoftBO.tipoexamenWS.TipoExamenWS.listarTodosTipoExamenAsync(SoftBO.tipoexamenWS.listarTodosTipoExamenRequest request) {
            return base.Channel.listarTodosTipoExamenAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftBO.tipoexamenWS.listarTodosTipoExamenResponse> listarTodosTipoExamenAsync() {
            SoftBO.tipoexamenWS.listarTodosTipoExamenRequest inValue = new SoftBO.tipoexamenWS.listarTodosTipoExamenRequest();
            return ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).listarTodosTipoExamenAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftBO.tipoexamenWS.obtenerPorIdTipoExamenResponse SoftBO.tipoexamenWS.TipoExamenWS.obtenerPorIdTipoExamen(SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest request) {
            return base.Channel.obtenerPorIdTipoExamen(request);
        }
        
        public SoftBO.tipoexamenWS.tipoExamenDTO obtenerPorIdTipoExamen(int tipoExamenId) {
            SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest inValue = new SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest();
            inValue.tipoExamenId = tipoExamenId;
            SoftBO.tipoexamenWS.obtenerPorIdTipoExamenResponse retVal = ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).obtenerPorIdTipoExamen(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.obtenerPorIdTipoExamenResponse> SoftBO.tipoexamenWS.TipoExamenWS.obtenerPorIdTipoExamenAsync(SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest request) {
            return base.Channel.obtenerPorIdTipoExamenAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftBO.tipoexamenWS.obtenerPorIdTipoExamenResponse> obtenerPorIdTipoExamenAsync(int tipoExamenId) {
            SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest inValue = new SoftBO.tipoexamenWS.obtenerPorIdTipoExamenRequest();
            inValue.tipoExamenId = tipoExamenId;
            return ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).obtenerPorIdTipoExamenAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SoftBO.tipoexamenWS.insertarTipoExamenResponse SoftBO.tipoexamenWS.TipoExamenWS.insertarTipoExamen(SoftBO.tipoexamenWS.insertarTipoExamenRequest request) {
            return base.Channel.insertarTipoExamen(request);
        }
        
        public int insertarTipoExamen(SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen) {
            SoftBO.tipoexamenWS.insertarTipoExamenRequest inValue = new SoftBO.tipoexamenWS.insertarTipoExamenRequest();
            inValue.tipoExamen = tipoExamen;
            SoftBO.tipoexamenWS.insertarTipoExamenResponse retVal = ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).insertarTipoExamen(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SoftBO.tipoexamenWS.insertarTipoExamenResponse> SoftBO.tipoexamenWS.TipoExamenWS.insertarTipoExamenAsync(SoftBO.tipoexamenWS.insertarTipoExamenRequest request) {
            return base.Channel.insertarTipoExamenAsync(request);
        }
        
        public System.Threading.Tasks.Task<SoftBO.tipoexamenWS.insertarTipoExamenResponse> insertarTipoExamenAsync(SoftBO.tipoexamenWS.tipoExamenDTO tipoExamen) {
            SoftBO.tipoexamenWS.insertarTipoExamenRequest inValue = new SoftBO.tipoexamenWS.insertarTipoExamenRequest();
            inValue.tipoExamen = tipoExamen;
            return ((SoftBO.tipoexamenWS.TipoExamenWS)(this)).insertarTipoExamenAsync(inValue);
        }
    }
}
