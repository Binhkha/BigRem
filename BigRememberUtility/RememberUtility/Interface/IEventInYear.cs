﻿using System.Collections.Generic;
using ConnectionSampleCode.Model;

namespace ConnectionSampleCode.Interface
{
    public interface IEventInYear
    {
        void AddEvent(EventInYear eventInYear);
        EventInYear FindEventInYear(string eventName);

        bool UpdateEvent(string currentEventName, string newEventName, string newCountryOccured, string newEventLink,
            string newEventDate, string newShortDescribe);

        bool DeleteEvent(string eventName);

        List<EventInYear> GetEventInYears();
    }
}