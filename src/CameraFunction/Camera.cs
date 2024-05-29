using System;
using System.Collections.Generic;
using System.Linq;

// Define a generic interface IRange with a type constraint T that implements IComparable<T>
public interface IRange<T> where T : IComparable<T>
{
    T Start { get; set; }
    T End { get; set; }
    bool Contains(IRange<T> other); // Check if this range contains another range
    bool Overlaps(IRange<T> other); // Check if this range overlaps with another range
}

// Define an interface ICamera with properties for camera name, light level range, and subject distance range
public interface ICamera
{
    string Name { get; set; }
    IRange<int> LightLevelRange { get; set; }
    IRange<double> SubjectDistanceRange { get; set; }
}

// Implement the IRange<T> interface in the Range class
public class Range<T> : IRange<T> where T : IComparable<T>
{
    public T Start { get; set; }
    public T End { get; set; }

    // Constructor to initialize the start and end of the range
    public Range(T start, T end)
    {
        Start = start;
        End = end;
    }

    // Check if this range contains another range
    public bool Contains(IRange<T> other)
    {
        return Start.CompareTo(other.Start) <= 0 && End.CompareTo(other.End) >= 0;
    }

    // Check if this range overlaps with another range
    public bool Overlaps(IRange<T> other)
    {
        return Start.CompareTo(other.End) <= 0 && End.CompareTo(other.Start) >= 0;
    }

    public override string ToString()
    {
        return $"Range: {Start} to {End}";
    }
}

// Implement the ICamera interface in the Camera class
public class Camera : ICamera
{
    public string Name { get; set; }
    public IRange<int> LightLevelRange { get; set; }
    public IRange<double> SubjectDistanceRange { get; set; }

    // Constructor to initialize the camera name, light level range, and subject distance range
    public Camera(string name, IRange<int> lightLevelRange, IRange<double> subjectDistanceRange)
    {
        Name = name;
        LightLevelRange = lightLevelRange;
        SubjectDistanceRange = subjectDistanceRange;
    }

    public override string ToString()
    {
        return $"Camera: {Name}, Light Level Range: {LightLevelRange}, Subject Distance Range: {SubjectDistanceRange}";
    }
}

public class Program
{
    // Function to check if a required range can be covered by a list of available ranges
    public static bool CanCoverRange<T>(IRange<T> requiredRange, List<IRange<T>> availableRanges) where T : IComparable<T>
    {
        // Sort the available ranges by their start values
        var sortedRanges = availableRanges.OrderBy(r => r.Start).ToList();
        T currentEnd = requiredRange.Start;

        // Iterate through the sorted ranges to check coverage
        foreach (var range in sortedRanges)
        {
            // Check if the current range can extend the coverage
            if (range.Start.CompareTo(currentEnd) <= 0 && range.End.CompareTo(currentEnd) >= 0)
            {
                currentEnd = range.End;
            }

            // If the current end covers the required end, return true
            if (currentEnd.CompareTo(requiredRange.End) >= 0)
            {
                return true;
            }
        }

        // If the required range is not fully covered, return false
        return false;
    }

    // Function to check if the software camera can cover the required distance and light level ranges
    public static bool CanSoftwareCameraWork(
        IRange<double> requiredDistanceRange, 
        IRange<int> requiredLightLevelRange, 
        List<ICamera> hardwareCameras)
    {
        // Extract the distance and light level ranges from the list of hardware cameras
        var distanceRanges = hardwareCameras.Select(c => c.SubjectDistanceRange).ToList();
        var lightLevelRanges = hardwareCameras.Select(c => c.LightLevelRange).ToList();

        // Check if both the required distance and light level ranges can be covered
        bool distanceCovered = CanCoverRange(requiredDistanceRange, distanceRanges);
        bool lightLevelCovered = CanCoverRange(requiredLightLevelRange, lightLevelRanges);

        return distanceCovered && lightLevelCovered;
    }

    public static void Main()
    {
        // Define a list of hardware cameras with their respective ranges
        var cameras = new List<ICamera>
        {
            new Camera("Camera A", new Range<int>(100, 500), new Range<double>(0.5, 5.0)),
            new Camera("Camera B", new Range<int>(400, 1000), new Range<double>(4.0, 10.0)),
            new Camera("Camera C", new Range<int>(900, 1500), new Range<double>(9.0, 20.0))
        };

        // Define the required distance and light level ranges for the software camera
        var requiredDistanceRange = new Range<double>(0.5, 15.0);
        var requiredLightLevelRange = new Range<int>(100, 1500);

        // Check if the hardware cameras can cover the required ranges
        bool result = CanSoftwareCameraWork(requiredDistanceRange, requiredLightLevelRange, cameras);

        // Output the result
        Console.WriteLine(result ? "The hardware cameras can cover the required range." : "The hardware cameras cannot cover the required range.");
    }
}
