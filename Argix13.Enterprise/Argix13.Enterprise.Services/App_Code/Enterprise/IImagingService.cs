using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Argix.Enterprise {
    //Freight Interfaces
    [ServiceContract(Namespace = "http://Argix.Enterprise")]
    public interface IImagingService {
        //General Interface        
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        DocumentClasses GetDocumentClasses();

        [OperationContract(Name = "GetDocumentClassesByDepartment")]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        DocumentClasses GetDocumentClasses(string department);

        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        MetaDatas GetMetaData();

        [OperationContract(Name = "GetMetaDataByClassName")]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        MetaDatas GetMetaData(string className);

        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        DataSet SearchSharePointImageStore(SearchRequest request);
        
        [OperationContract]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        byte[] GetSharePointImageStream(SearchRequest request);

        [OperationContract(Name = "GetSharePointImageStreamByUri")]
        [FaultContractAttribute(typeof(EnterpriseFault),Action = "http://Argix.EnterpriseFault")]
        byte[] GetSharePointImageStream(string uri);
    }

     [DataContract]
    public class SearchRequest {
        //Members
        private string mScopeName="All Sites";
        private string mDocumentClass="", mPropertyName="", mPropertyValue="";
        private string mOperand1="", mPropertyName1="", mPropertyValue1="";
        private string mOperand2="", mPropertyName2="", mPropertyValue2="";
        private string mOperand3="", mPropertyName3="", mPropertyValue3="";
        private int mMaxResults=250;

        //Interface
        public SearchRequest() { }
        #region Members
        [DataMember]
        public string ScopeName { get { return this.mScopeName; } set { this.mScopeName = value; } }
        [DataMember]
        public string DocumentClass { get { return this.mDocumentClass; } set { this.mDocumentClass = value; } }
        [DataMember]
        public string PropertyName { get { return this.mPropertyName; } set { this.mPropertyName = value; } }
        [DataMember]
        public string PropertyValue { get { return this.mPropertyValue; } set { this.mPropertyValue = value; } }
        [DataMember]
        public string Operand1 { get { return this.mOperand1; } set { this.mOperand1 = value; } }
        [DataMember]
        public string PropertyName1 { get { return this.mPropertyName1; } set { this.mPropertyName1 = value; } }
        [DataMember]
        public string PropertyValue1 { get { return this.mPropertyValue1; } set { this.mPropertyValue1 = value; } }
        [DataMember]
        public string Operand2 { get { return this.mOperand2; } set { this.mOperand2 = value; } }
        [DataMember]
        public string PropertyName2 { get { return this.mPropertyName2; } set { this.mPropertyName2 = value; } }
        [DataMember]
        public string PropertyValue2 { get { return this.mPropertyValue2; } set { this.mPropertyValue2 = value; } }
        [DataMember]
        public string Operand3 { get { return this.mOperand3; } set { this.mOperand3 = value; } }
        [DataMember]
        public string PropertyName3 { get { return this.mPropertyName3; } set { this.mPropertyName3 = value; } }
        [DataMember]
        public string PropertyValue3 { get { return this.mPropertyValue3; } set { this.mPropertyValue3 = value; } }
        [DataMember]
        public int MaxResults { get { return this.mMaxResults; } set { this.mMaxResults = value; } }
        #endregion
    }

    [CollectionDataContract]
    public class DocumentClasses:BindingList<DocumentClass> { public DocumentClasses() { } }

    [DataContract]
    public class DocumentClass {
        //Members
        private string mDepartment = "",mClassName = "";

        //Interface
        public DocumentClass() { }
        public DocumentClass(string department,string classname) { this.mDepartment = department; this.mClassName = classname; }

        [DataMember]
        public string Department { get { return this.mDepartment; } set { this.mDepartment = value; } }
        [DataMember]
        public string ClassName { get { return this.mClassName; } set { this.mClassName = value; } }
    }

    [CollectionDataContract]
    public class MetaDatas:BindingList<MetaData> { public MetaDatas() { }  }

    [DataContract]
    public class MetaData {
        //Members
        private string mClassName = "",mProperty = "",mValue = "";

        //Interface
        public MetaData() { }
        public MetaData(string classname,string property,string value) { this.mClassName = classname; this.mProperty = property; this.mValue = value; }

        [DataMember]
        public string ClassName { get { return this.mClassName; } set { this.mClassName = value; } }
        [DataMember]
        public string Property { get { return this.mProperty; } set { this.mProperty = value; } }
        [DataMember]
        public string Value { get { return this.mValue; } set { this.mValue = value; } }
    }
}
