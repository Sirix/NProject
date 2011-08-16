using System.Web.Mvc;

namespace NProject.Helpers
{
    public static class ControllerExtension
    {
        /// <summary>
        /// Outputs temporary message to user.
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="message">Message text</param>
        /// <param name="messageLevel">Message level - error, information</param>
        public static void SetTempMessage(this Controller controller, string message, string messageLevel)
        {
            controller.TempData[messageLevel + "Message"] = message;
        }
    }
}