namespace SliderPlaybackVisualization
{
    class GroupedModel
    {
        public PieViewModel PieViewModel { get; set; }
        public TimelineModel TimelineModel { get; set; }

        public GroupedModel ()
        {
            PieViewModel = new PieViewModel();
            TimelineModel = new TimelineModel();
        }
    }
}
