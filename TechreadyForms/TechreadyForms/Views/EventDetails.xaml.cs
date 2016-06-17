using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechreadyForms.Helpers;
using TechreadyForms.Helpers.ShareHelper;
using TechReady.Common.Models;
using TechReady.Helpers.Storage;
using TechReady.ViewModels;
using Xamarin.Forms;

namespace TechreadyForms.Views
{
    public partial class EventDetails : ContentPage
    {
        private ToolbarItem followToolbarItem;

        private ToolbarItem unFollowToolbarItem;

        private ToolbarItem shareToolbarItem;


        private Event _parameter;

        public EventDetails(Event parameter)
        {
            _parameter = parameter;
            this.BindingContext = new EventDetailsPageViewModel();

            this.ViewModel.FollowedChanged += ViewModel_FollowedChanged;

            followToolbarItem = new ToolbarItem("Follow", "Follow.png", () =>
            {
                this.Follow_Click(this, new EventArgs());
            }
            );

            unFollowToolbarItem = new ToolbarItem("UnFollow", "UnFollow.png", () =>
            {
                this.Follow_Click(this, new EventArgs());
            }
                );

            shareToolbarItem = new ToolbarItem("Share","Share.png",this.ShareEvent);

            this.ToolbarItems.Add(shareToolbarItem);

            InitializeComponent();

            this.NavigatedTo();
        }

        private void ViewModel_FollowedChanged()
        {
            if (this.ViewModel.Event.IsFollowed)
            {
                if (!this.ToolbarItems.Contains(unFollowToolbarItem))
                    this.ToolbarItems.Add(unFollowToolbarItem);
                if (this.ToolbarItems.Contains(followToolbarItem))
                    this.ToolbarItems.Remove(followToolbarItem);
            }
            else
            {
                if (!this.ToolbarItems.Contains(followToolbarItem))
                    this.ToolbarItems.Add(followToolbarItem);
                if (this.ToolbarItems.Contains(unFollowToolbarItem))
                    this.ToolbarItems.Remove(unFollowToolbarItem);
            }
        }

        public EventDetailsPageViewModel ViewModel
        {
            get { return this.BindingContext as EventDetailsPageViewModel; }
        }


        private async void PopupateTrack()
        {
            this.AgendaStack.Children.Clear();

            if (this.ViewModel.Event != null && this.ViewModel.Event.CurrentTrack != null &&
                this.ViewModel.Event.CurrentTrack.TrackSessionsDated != null)
            {

                foreach (var trackSessionsDatedView in this.ViewModel.Event.CurrentTrack.TrackSessionsDated)
                {
                    this.AgendaStack.Children.Add(new Label()
                    {
                        Text = trackSessionsDatedView.TrackDateString,
                        FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.Bold
                    });

                    var trackAgendaDetailsStack = new StackLayout()
                    {
                        Spacing = 2
                    };
                    foreach (var trackDateSession in trackSessionsDatedView.TrackDateSessions)
                    {
                        var grid = new Grid()
                        {
                            ColumnDefinitions = new ColumnDefinitionCollection()
                            {
                                new ColumnDefinition()
                                {
                                    Width = new GridLength(1, GridUnitType.Star),
                                },
                                new ColumnDefinition()
                                {
                                    Width = new GridLength(12),
                                },
                                new ColumnDefinition()
                                {
                                    Width = new GridLength(110),
                                }
                            },
                            HeightRequest = 35

                        };
                        var titleLabel = new CustomLabel()
                        {
                            Text = trackDateSession.Title,
                            LineBreakMode = LineBreakMode.WordWrap,
                            TextColor = Color.Black,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                            MaxLines = 2

                        };
                        Grid.SetColumn(titleLabel,0);
                        var colonLabel = new Label()
                        {
                            Text = ":",
                            LineBreakMode = LineBreakMode.WordWrap,
                            TextColor = Color.Black,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))

                        };
                        Grid.SetColumn(colonLabel, 1);
                        var timeLabel = new Label()
                        {
                            Text = trackDateSession.SessionFromTo,
                            LineBreakMode = LineBreakMode.WordWrap,
                            TextColor = Color.Black,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                            HorizontalOptions = LayoutOptions.Start,
                            
                        };
                        Grid.SetColumn(timeLabel, 2);

                        grid.Children.Add(titleLabel);
                        grid.Children.Add(colonLabel);
                        grid.Children.Add(timeLabel);


                        var tapRecognizer = 
                        new TapGestureRecognizer();
                        tapRecognizer.Tapped += (sender, args) =>
                        {
                            Navigation.PushAsync(new SessionDetailsPage(trackDateSession));
                        };

                        grid.GestureRecognizers.Add(tapRecognizer);
                        

                        trackAgendaDetailsStack.Children.Add(grid);
                    }
                    this.AgendaStack.Children.Add(trackAgendaDetailsStack);

                }
            }

        }


        private async void NavigatedTo()
        {
            if (_parameter != null)
            {

                this.ViewModel.Event = _parameter;
                this.ViewModel_FollowedChanged();

                this.PopupateTrack();
                
               
                
                var data = await LocalStorage.ReadJsonFromFile<List<Event>>("followedEvents");
                if (data == null)
                {
                    data = new List<Event>();
                }


                if (this.ViewModel == null)
                {
                    return;
                }

                if (data.FirstOrDefault(x => x.EventId == this.ViewModel.Event.EventId) != null)
                {
                    this.ViewModel.Event.IsFollowed = true;
                }
                
            }

          
        }


        private async void PromptUserToFollow()
        {
            await
                TechReady.Helpers.MessageHelper.MessageHelper.ShowMessage(
                    "Will you like to follow this event to receive updates?", "", "Ok", "Cancel",
                    ContentDialog_PrimaryButtonClick);

            //MessageContentDialog msgDialog = new MessageContentDialog("Will you like to follow this event to receive updates?");
            //msgDialog.PrimaryButtonText = "Ok";
            //msgDialog.SecondaryButtonText = "Cancel";
            //msgDialog.PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
            //await msgDialog.ShowMessage();
        }


        private async void ContentDialog_PrimaryButtonClick()
        {
            await this.ViewModel.Follow_Click(null);
        }

        private async void Follow_Click(object sender, EventArgs e)
        {
            await this.ViewModel.Follow_Click(sender);
        }

        private void Track_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopupateTrack();
        }

        private void Register_OnClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ViewModel.Event.EventRegLink))
            {
                TechReady.Helpers.CommonSettings.FromRegisterationPage = true; // flag 

                Navigation.PushAsync(new WebViewPage(
                   this.ViewModel.Event.EventRegLink)
                );


            }
        }

        private void EventDetails_OnAppearing(object sender, EventArgs e)
        {
            if (TechReady.Helpers.CommonSettings.FromRegisterationPage)
            {
                if (!this.ViewModel.Event.IsFollowed)
                    this.PromptUserToFollow();
                TechReady.Helpers.CommonSettings.FromRegisterationPage = false;
            }
        }


        public void ShareEvent()
        {

            try
            {
                this.ViewModel.OperationInProgress = true;
                StringBuilder builder = new StringBuilder();
                builder.Append("I am attending");
                builder.Append("\n");
                builder.Append(string.Format("{0}, registration Link {1}", this.ViewModel.Event.EventName,
                    this.ViewModel.Event.EventRegLink));


                ShareHelper.Share(builder.ToString());
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.ViewModel.OperationInProgress = false;
            }
        }

        private void Speaker_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && e.Item is TrackSpeaker)
            {
                Navigation.PushAsync(new SpeakerDetailsPage((TrackSpeaker)e.Item));
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}
