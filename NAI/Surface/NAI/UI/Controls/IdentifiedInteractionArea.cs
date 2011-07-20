using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using NAI.UI.Client;
using NAI.Client;
using NAI.Client.Calibration;
using System.Linq;
using NAI.Communication;

namespace NAI.UI.Controls
{
    public class IdentifiedInteractionArea : TagVisualizer
    {
        private static HashSet<DependencyObject> killEventEnabledIdentifiedDependencyObjects = new HashSet<DependencyObject>();
        private static HashSet<DependencyObject> identifiedDependencyObjectRoots = new HashSet<DependencyObject>();

        // Keep track of the tags being visualized
        private HashSet<TagData> tags = new HashSet<TagData>();

        private Server _server;

        public IdentifiedInteractionArea()
        {
            _server = Server.Instance;

            AddHandler(TagVisualization.LostTagEvent, new RoutedEventHandler(OnTagLost));

            // Add event handler to override the needed events to enfore identified access to UIelements
            this.AddHandler(Contacts.PreviewContactDownEvent, new ContactEventHandler(onPreviewContactDownEvent));
            this.AddHandler(Contacts.PreviewContactChangedEvent, new ContactEventHandler(TagVisualizer_PreviewContactChanged));
        }


        /// <summary>
        /// Accept any tag that is not already visualized.
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        protected override TagVisualization CreateVisualizationForTag(Contact contact)
        {
            if (contact.IsTagRecognized)
            {
                bool tagInUse = tags.Contains(contact.Tag);
                if (!tagInUse)
                {
                    tags.Add(contact.Tag); 
                    return new ClientTagVisualization();
                }
            }
            return null;
        }

        private void OnTagLost(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TagVisualization)
            {
                tags.Remove(((TagVisualization)e.OriginalSource).VisualizedTag);
            }
        }

        private void TagVisualizer_PreviewContactChanged(object sender, ContactEventArgs e)
        {
            // Update the tag orientation for calibrations
            if (e.Contact.IsTagRecognized)
            {
                TagData tag = e.Contact.Tag;
                if (ClientSessionsController.Instance.GetClientState(tag) is CalibrationState)
                {
                    TagVisualization tagVisualization = FindVisualization(tag);
                    if (tagVisualization != null)
                    {
                        ClientTagVisualization clientVisualization = tagVisualization as ClientTagVisualization;
                        CalibrationUserControl cuc = clientVisualization.MyContent as CalibrationUserControl;
                        if (cuc != null)
                        {
                            double orientation = e.Contact.GetOrientation(cuc);
                            Point newPos = this.TranslatePoint(e.Contact.GetCenterPosition(this), clientVisualization);
                            Debug.WriteLineIf(DebugSettings.DEBUG_CALIBRATION, string.Format("Tag Moved: Position: {0}. Rotation: {1}.", newPos, orientation));                            cuc.TagPosition = newPos;
                            cuc.TagOrientation = orientation;
                            e.Handled = true; // Ensure that the calibration control is not moved
                        }
                    }
                }
            }
        }

        private TagVisualization FindVisualization(TagData tag)
        {
            return this.ActiveVisualizations.FirstOrDefault(x => x.VisualizedTag.Equals(tag));
        }

        public bool HasTagVisualizationForTag(TagData tag)
        {
            return FindVisualization(tag) == null;
        }


        /// <summary>
        /// Finds the first RestrictedTagVisualizer ancestor in the visual tree for the given FrameworkElement
        /// and returns the transformation from the RestrictedTagVisualizer to the given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static GeneralTransform GetTransformTo(FrameworkElement element)
        {
            if (element.Parent == null || !(element.Parent is FrameworkElement) || element is IdentifiedInteractionArea)
            {
                return null;
            }

            FrameworkElement parent = element.Parent as FrameworkElement;

            while (parent != null && parent is FrameworkElement && !(parent is IdentifiedInteractionArea))
            {
                parent = parent.Parent as FrameworkElement;
            }

            if (parent != null)
            {
                return parent.TransformToDescendant(element);
            }
            return null;
        }

        # region Identified UI Elements

        public static void KillEventsForIdentifiedUIElement(UIElement element)
        {
            Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "AddIdentifiedUIElement:" + element.ToString());
            if (killEventEnabledIdentifiedDependencyObjects.Contains(element))
                return;
            killEventEnabledIdentifiedDependencyObjects.Add(element);
        }

        public static void StopKillEventsForIdentifiedUIElement(UIElement element)
        {
            if (!killEventEnabledIdentifiedDependencyObjects.Contains(element))
                return;
            killEventEnabledIdentifiedDependencyObjects.Remove(element);
        }

        public static void AddIdentifiedDependencyObjectRoot(DependencyObject element)
        {
            if (identifiedDependencyObjectRoots.Contains(element))
                return;
            identifiedDependencyObjectRoots.Add(element);
        }

        public static void RemoveIdentifiedDependencyObjectRoot(DependencyObject element)
        {            
            identifiedDependencyObjectRoots.Remove(element);
        }

        private bool ShouldHandleEvent(UIElement source)
        {
            return FindLogicalRegisteredAncestor(source, killEventEnabledIdentifiedDependencyObjects) != null;
        }

        public DependencyObject GetRegisteredIdentifiedAncestor(DependencyObject element)
        {
            return FindLogicalRegisteredAncestor(element, identifiedDependencyObjectRoots);
        }

        private DependencyObject FindLogicalRegisteredAncestor(DependencyObject e, HashSet<DependencyObject> depObjects)
        {
            if (e == null)
                return null;
            else if (depObjects.Contains(e))
                return e;

            DependencyObject result = LogicalTreeHelper.GetParent(e);
            if (result == null)
                return FindLogicalRegisteredAncestor(VisualTreeHelper.GetParent(e), depObjects);
            else if (depObjects.Contains(result))
                return result;
            return FindLogicalRegisteredAncestor(result, depObjects);
        }

        # endregion

        #region EventHandlers

        private void onPreviewContactDownEvent(object sender, ContactEventArgs e)
        {
            if (ShouldHandleEvent(e.Source as UIElement) || ShouldHandleEvent(e.OriginalSource as UIElement))
            {
                e.Handled = true;
                Debug.WriteLineIf(DebugSettings.DEBUG_EVENTS, "Killed PreviewContactDownEvent with source " + e.Source.ToString());
            }
        }

        #endregion

    }
}
