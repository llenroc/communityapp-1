using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechReady.Common.DTO;
using TechReady.Helpers.MessageHelper;
using TechReady.Helpers.ServiceCallers;
using TechReady.Helpers.Storage;

namespace TechReady.ViewModels
{
   public class FeedbackPageViewModel : BaseViewModel
    {
     
        public FeedbackPageViewModel()
        {
            this._feedbackTypes = new List<string>()
            {
                "Bug Report","Feature Request","Other"
            };
        }
        
        private string _userName { get; set; }
        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                if(_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        private string _email { get; set; }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if(_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }


        private List<string> _feedbackTypes;
        public List<string> FeedbackTypes
        {
            get
            {
                return _feedbackTypes;
            }
            set
            {
                if (_feedbackTypes != value)
                {
                    _feedbackTypes = value;
                    OnPropertyChanged("FeedbackTypes");
                }
            }
        }


        private string _selectedFeedbackType;
        public string SelectedFeedbackType
        {
            get
            {
                return _selectedFeedbackType;
            }
            set
            {
                if(_selectedFeedbackType != value)
                {
                    _selectedFeedbackType = value;
                    OnPropertyChanged("SelectedFeedbackType");
                }
            }
        }

        private string _feedback;
        public string Feedback
        {
            get
            {
                return _feedback;
            }
            set
            {
                if(_feedback != value)
                {
                    _feedback = value;
                    OnPropertyChanged("Feedback");
                }
            }
        }

        private string _responseMessage;
        public string ResponseMessage
        {
            get
            {
                return _responseMessage;
            }

            set
            {
                if(_responseMessage != value)
                {
                    _responseMessage = value;
                    OnPropertyChanged("ResponseMessage");
                    OnPropertyChanged("ShowMessage");
                }
            }
        }

        public bool ShowMessage
        {
            get
            {
                return (!string.IsNullOrEmpty(this.ResponseMessage));
            }
        }

        public async Task<bool> SaveFeedback()
        {

            if (string.IsNullOrEmpty(this.UserName))
            {
                await MessageHelper.ShowMessage("Please provide your Name");
                return false;
            }

            else if (string.IsNullOrEmpty(this.Email))
            {
                await MessageHelper.ShowMessage("Please provide your Email");
                return false;
            }

            else if (this.SelectedFeedbackType == null)
            {
                await MessageHelper.ShowMessage("Please provide your FeedbackType");
                return false;
            }


            else if (string.IsNullOrWhiteSpace(this.Feedback))
            {
                await MessageHelper.ShowMessage("Please provide your Feedback");
                return false;
            }



            string email = this.Email;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                await MessageHelper.ShowMessage("Please enter valid email address");
                return false;
            }



            try
            {
                this.OperationInProgress = true;
                if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == false)
                {
                    await MessageHelper.ShowMessage("Please connect to internet to submit feedback");
                    return false;
                }
                    // Fetch data about the user
                    //saves and updates data on server
                    var model = await LocalStorage.ReadJsonFromFile<UserRegistrationPageViewModel>("registration");
                if (model != null)
                {
                    if (Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable() == true)
                    {
                        //TODO SHIV: API Check
                        var result = await ServiceProxy.CallService("api/Feedback", JsonConvert.SerializeObject(
                    new FeedbackRequest()
                    {
                        Name = this.UserName,
                        Email = this.Email,
                        FeedbackType = this.SelectedFeedbackType,
                        Feedback = this.Feedback,
                        AppUserId = model.UserId
                    }));
                        
                        if (result.IsSuccess)
                        {
                            FeedbackResponse response = JsonConvert.DeserializeObject<FeedbackResponse>(result.response);
                            // await MessageHelper.ShowMessage(response.ResponseText);
                            this.ResponseMessage = response.ResponseText;
                            return true;
                        }

                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                this.OperationInProgress = false;
            }
           
        }
    }
}
