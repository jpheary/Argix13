using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Threading;

namespace Argix.Enterprise {
	//
	public class ImagingGateway {
		//Members

		//Interface
        public ImagingGateway() { }
        public CommunicationState ServiceState { get { return new ImagingServiceClient().State; } }
        public string ServiceAddress { get { return new ImagingServiceClient().Endpoint.Address.Uri.AbsoluteUri; } }

        public DocumentClasses GetDocumentClasses(string department) {
            //
            DocumentClasses classes = null;
            ImagingServiceClient client = null;
            try {
                client = new ImagingServiceClient();
                classes = client.GetDocumentClassesByDepartment(department);
                client.Close();
            }
            catch (TimeoutException te) { client.Abort(); throw new ApplicationException(te.Message); }
            catch (FaultException<EnterpriseFault> ef) { client.Abort(); throw new ApplicationException(ef.Detail.Message); }
            catch (FaultException fe) { client.Abort(); throw new ApplicationException(fe.Message); }
            catch (CommunicationException ce) { client.Abort(); throw new ApplicationException(ce.Message); }
            return classes;
        }
        public MetaDatas GetMetaData(string className) {
            //
            MetaDatas metaData = null;
            ImagingServiceClient client = null;
            try {
                client = new ImagingServiceClient();
                metaData = client.GetMetaDataByClassName(className);
                client.Close();
            }
            catch (TimeoutException te) { client.Abort(); throw new ApplicationException(te.Message); }
            catch (FaultException<EnterpriseFault> ef) { client.Abort(); throw new ApplicationException(ef.Detail.Message); }
            catch (FaultException fe) { client.Abort(); throw new ApplicationException(fe.Message); }
            catch (CommunicationException ce) { client.Abort(); throw new ApplicationException(ce.Message); }
            return metaData;
        }
        public DataSet SearchSharePointImageStore(SearchRequest request) {
            //
            DataSet response = null;
            ImagingServiceClient client = null;
            try {
                client = new ImagingServiceClient();
                response = client.SearchSharePointImageStore(request);
                client.Close();
            }
            catch (TimeoutException te) { client.Abort(); throw new ApplicationException(te.Message); }
            catch (FaultException<EnterpriseFault> ef) { client.Abort(); throw new ApplicationException(ef.Detail.Message); }
            catch (FaultException fe) { client.Abort(); throw new ApplicationException(fe.Message); }
            catch (CommunicationException ce) { client.Abort(); throw new ApplicationException(ce.Message); }
            return response;
        }
        public byte[] GetSharePointImageStream(SearchRequest request) {
            //
            byte[] bytes = null;
            ImagingServiceClient client = null;
            try {
                client = new ImagingServiceClient();
                bytes = client.GetSharePointImageStream(request);
                client.Close();
            }
            catch (TimeoutException te) { client.Abort(); throw new ApplicationException(te.Message); }
            catch (FaultException<EnterpriseFault> ef) { client.Abort(); throw new ApplicationException(ef.Detail.Message); }
            catch (FaultException fe) { client.Abort(); throw new ApplicationException(fe.Message); }
            catch (CommunicationException ce) { client.Abort(); throw new ApplicationException(ce.Message); }
            return bytes;
        }
    }
}