using Prism.Commands;

namespace WindowsClient.Features.Transmissibility
{
    public class TransmissibilityViewModel 
    {       
        public TransmissibilityViewModel(ITransmissibilityWindow transmissibilityView)
        {
            TransmissibilityView = transmissibilityView;            
        }
        public ITransmissibilityWindow TransmissibilityView { get; }
        public DelegateCommand TransmissibilityWindowCommand { get; set; }
    }
}
