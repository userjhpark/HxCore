using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HxCore.Win
{
    public interface IHxMessageBox
    {
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   displayHelpButton:
        //     도움말 단추를 표시하려면 true이고, 그렇지 않으면 false입니다. 기본값은 false입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton);
        //
        // 요약:
        //     지정된 텍스트, 캡션 및 단추가 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons);
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추 및 아이콘이 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추, 아이콘 및 기본 단추가 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 구성원이 아닙니다 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추 및 옵션이 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 구성원이 아닙니다 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 options 지정 된 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 또는
        //     System.Windows.Forms.MessageBoxOptions.ServiceNotification 가 값을 지정 하 고는 owner
        //     매개 변수입니다. 이 메서드를 사용 하지 않는 버전을 호출 하는 경우에 이러한 두 옵션을 사용할지는 owner 매개 변수입니다. 또는 buttons
        //     잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
        //
        // 요약:
        //     지정된 텍스트가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        DialogResult Show(string text);
        //
        // 요약:
        //     지정된 텍스트와 캡션이 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        DialogResult Show(string text, string caption);
        //
        // 요약:
        //     지정된 텍스트, 캡션 및 단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 매개 변수가 지정의 구성원이 아닙니다. System.Windows.Forms.MessageBoxButtons합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons);
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추 및 아이콘이 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 매개 변수가 지정의 구성원이 아닙니다. System.Windows.Forms.MessageBoxButtons합니다. 또는 icon
        //     매개 변수가 지정의 구성원이 아닙니다. System.Windows.Forms.MessageBoxIcon합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        //
        // 요약:
        //     지정된 텍스트와 캡션이 있는 메시지 상자를 지정된 개체 앞에 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        DialogResult Show(IWin32Window owner, string text, string caption);
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추, 아이콘 및 기본 단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 구성원이 아닙니다 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
        //
        // 요약:
        //     지정된 도움말 파일, HelpNavigator 및 도움말 항목을 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말
        //     단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        //   navigator:
        //     System.Windows.Forms.HelpNavigator 값 중 하나입니다.
        //
        //   param:
        //     사용자가 도움말 단추를 클릭할 때 표시할 도움말 항목의 숫자 ID입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);
        //
        // 요약:
        //     지정된 도움말 파일, HelpNavigator 및 도움말 항목을 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말
        //     단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        //   navigator:
        //     System.Windows.Forms.HelpNavigator 값 중 하나입니다.
        //
        //   param:
        //     사용자가 도움말 단추를 클릭할 때 표시할 도움말 항목의 숫자 ID입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param);
        //
        // 요약:
        //     지정된 도움말 파일 및 HelpNavigator를 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는
        //     메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        //   navigator:
        //     System.Windows.Forms.HelpNavigator 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);
        //
        // 요약:
        //     지정된 도움말 파일 및 HelpNavigator를 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는
        //     메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        //   navigator:
        //     System.Windows.Forms.HelpNavigator 값 중 하나입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator);
        //
        // 요약:
        //     지정된 도움말 파일 및 도움말 키워드를 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는 메시지 상자를
        //     표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        //   keyword:
        //     사용자가 도움말 단추를 클릭할 때 표시할 도움말 키워드입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword);
        //
        // 요약:
        //     지정된 도움말 파일 및 도움말 키워드를 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는 메시지 상자를
        //     표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        //   keyword:
        //     사용자가 도움말 단추를 클릭할 때 표시할 도움말 키워드입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword);
        //
        // 요약:
        //     지정된 도움말 파일을 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);
        //
        // 요약:
        //     지정된 도움말 파일을 사용하여 지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추, 옵션 및 도움말 단추가 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        //   helpFilePath:
        //     사용자가 도움말 단추를 클릭할 경우 표시할 도움말 파일의 경로와 이름입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath);
        //
        // 요약:
        //     지정된 텍스트, 캡션, 단추, 아이콘, 기본 단추 및 옵션이 있는 메시지 상자를 표시합니다.
        //
        // 매개 변수:
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        //   caption:
        //     메시지 상자의 제목 표시줄에 표시할 텍스트입니다.
        //
        //   buttons:
        //     메시지 상자에 표시할 단추를 지정하는 System.Windows.Forms.MessageBoxButtons 값 중 하나입니다.
        //
        //   icon:
        //     메시지 상자에 표시할 아이콘을 지정하는 System.Windows.Forms.MessageBoxIcon 값 중 하나입니다.
        //
        //   defaultButton:
        //     메시지 상자에 대한 기본 단추를 지정하는 System.Windows.Forms.MessageBoxDefaultButton 값 중 하나입니다.
        //
        //   options:
        //     메시지 상자에 사용할 표시 옵션과 연결 옵션을 지정하는 System.Windows.Forms.MessageBoxOptions 값 중 하나입니다.
        //     기본값을 사용하려면 0을 전달합니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        //
        // 예외:
        //   T:System.ComponentModel.InvalidEnumArgumentException:
        //     buttons 구성원이 아닙니다 System.Windows.Forms.MessageBoxButtons합니다. 또는 icon 구성원이 아닙니다
        //     System.Windows.Forms.MessageBoxIcon합니다. 또는 defaultButton 의 구성원이 아닙니다 지정 System.Windows.Forms.MessageBoxDefaultButton합니다.
        //
        //   T:System.InvalidOperationException:
        //     표시 하려고 했습니다는 System.Windows.Forms.MessageBox 사용자 대화형 모드로 실행 하지 않는 프로세스에 있습니다.
        //     이것은가 지정 된 System.Windows.Forms.SystemInformation.UserInteractive 속성입니다.
        //
        //   T:System.ArgumentException:
        //     options 둘 다 지정 System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly 및 System.Windows.Forms.MessageBoxOptions.ServiceNotification합니다.
        //     또는 buttons 잘못 된 조합이 지정 System.Windows.Forms.MessageBoxButtons합니다.
        DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
        //
        // 요약:
        //     지정된 텍스트를 포함하는 메시지 상자를 지정된 개체 앞에 표시합니다.
        //
        // 매개 변수:
        //   owner:
        //     모달 대화 상자를 소유할 System.Windows.Forms.IWin32Window의 구현입니다.
        //
        //   text:
        //     메시지 상자에 표시할 텍스트입니다.
        //
        // 반환 값:
        //     System.Windows.Forms.DialogResult 값 중 하나입니다.
        DialogResult Show(IWin32Window owner, string text);
    }
}
