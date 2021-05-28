using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIDC_Development_Helper
{
    /// <summary>
    /// Name: SIDC DLL Common Message Helper
    /// <para/>
    /// Developer: Angelito D. De Sagun
    /// <para/>
    /// Date: 2021-05-23
    /// <para/>
    /// Revision Date:--
    /// </summary>
    public class MessageHelper
    {
        /// <summary>
        /// Call this method to prompt information message
        /// </summary>
        /// <param name="Message"></param>
        public static void Information(string Message)
        {
            MessageBox.Show(Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Call this method to prompt error message
        /// </summary>
        /// <param name="Message"></param>
        public static void Error(string Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Call this method to prompt exclamation message
        /// </summary>
        /// <param name="Message"></param>
        public static void Exclamation(string Message)
        {
            MessageBox.Show(Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Call this method to prompt question information
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static DialogResult Question(string Message)
        {
            return MessageBox.Show(Message, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        /// <summary>
        /// Call this method prompt warning question
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static DialogResult WarningQuestion(string Message)
        {
            return MessageBox.Show(Message, "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        }
    }
}
