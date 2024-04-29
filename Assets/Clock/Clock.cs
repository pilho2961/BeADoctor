using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClockSample
{

	public class Clock : MonoBehaviour
    {
        public Transform handHours;
        public Transform handMinutes;
        public Transform handSeconds;

        // Start time and flow speed variables
        public int startHour = 9; // Default start hour
        public int startMinute = 0; // Default start minute
        public float flowSpeed = 27; // Set flow speed to three times faster than real time

        // Variables to track elapsed time
        private int elapsedHours;
        private int elapsedMinutes;
        private float elapsedSeconds;

        private void Start()
        {
            startHour = (int)DayNightCycle.Instance.timeOfDay;
            startMinute = (int)((DayNightCycle.Instance.timeOfDay - startHour) * 60);

            // Calculate initial elapsed time based on start time
            CalculateElapsedTime();

            // Update clock initially
            UpdateHands();

            // Update clock every second, adjust interval based on flow speed
            InvokeRepeating(nameof(UpdateHands), 0, 1 / flowSpeed);
        }

        void UpdateHands()
        {
            // Update elapsed time
            UpdateElapsedTime();

            // Convert elapsed time to hand rotation
            float handRotationHours = elapsedHours * 30;    // 360/12 = 30
            float handRotationMinutes = elapsedMinutes * 6; // 360/60 = 6
            float handRotationSeconds = elapsedSeconds * 6; // 360/60 = 6

            //Debug.Log("Elapsed time: " + elapsedHours + ":" + elapsedMinutes + ":" + elapsedSeconds);
            //Debug.Log("Hand rotation - Hours: " + handRotationHours + ", Minutes: " + handRotationMinutes + ", Seconds: " + handRotationSeconds);

            // Rotate the hand Transforms
            if (handHours)
            {
                handHours.localRotation = Quaternion.Euler(0, 0, handRotationHours);
                //Debug.Log("Hour hand rotation: " + handHours.localRotation.eulerAngles.z);
            }

            if (handMinutes)
            {
                handMinutes.localRotation = Quaternion.Euler(0, 0, handRotationMinutes);
                //Debug.Log("Minute hand rotation: " + handMinutes.localRotation.eulerAngles.z);
            }

            if (handSeconds)
            {
                handSeconds.localRotation = Quaternion.Euler(0, 0, handRotationSeconds);
                //Debug.Log("Second hand rotation: " + handSeconds.localRotation.eulerAngles.z);
            }
        }

        // Calculate initial elapsed time based on start time
        void CalculateElapsedTime()
        {
            // Calculate total elapsed seconds from start time
            int totalElapsedSeconds = startHour * 3600 + startMinute * 60;

            // Update elapsed hours, minutes, and seconds
            elapsedHours = totalElapsedSeconds / 3600;
            elapsedMinutes = (totalElapsedSeconds % 3600) / 60;
            elapsedSeconds = totalElapsedSeconds % 60;

            //Debug.Log("Initial elapsed time: " + elapsedHours + ":" + elapsedMinutes + ":" + elapsedSeconds);
        }

        // Update elapsed time based on flow speed
        void UpdateElapsedTime()
        {
            // Increment elapsed seconds based on flow speed
            //elapsedSeconds += Mathf.RoundToInt(Time.deltaTime * flowSpeed);
            elapsedSeconds += Time.deltaTime * flowSpeed;
            print(Time.deltaTime);
            print(elapsedSeconds);

            // Calculate overflow minutes and hours
            int overflowMinutes = (int)(elapsedSeconds / 60);
            elapsedSeconds %= 60;
            elapsedMinutes += overflowMinutes;
            int overflowHours = elapsedMinutes / 60;
            elapsedMinutes %= 60;
            elapsedHours = (elapsedHours + overflowHours) % 24; // Hours roll over every 24 hours

            //Debug.Log("Elapsed time: " + elapsedHours + ":" + elapsedMinutes + ":" + elapsedSeconds);
        }

        private void OnDestroy()
        {
            // Cancel the InvokeRepeating when the object is destroyed
            CancelInvoke();
        }
    }
}