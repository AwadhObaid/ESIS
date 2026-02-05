''' <summary>
''' أحداث التحقق من البوليصات للاستخدام في SaderForm - نسخة مُصلحة وآمنة
''' </summary>
Public Module PolicyValidationEvents

    Private _willMasterKeyDownHandler As KeyEventHandler
    Private _willSubKeyDownHandler As KeyEventHandler

    ''' <summary>
    ''' حدث KeyDown للتحقق من البوليصة الرئيسية عند الضغط على Enter
    ''' </summary>
    ''' <param name="sender">txtWillMaster</param>
    ''' <param name="e">معاملات الحدث</param>
    ''' <param name="parentForm">النموذج الأب (SaderForm)</param>
    Public Sub txtWillMaster_KeyDown(sender As Object, e As KeyEventArgs, parentForm As Form)
        Try
            If e.KeyCode <> Keys.Enter Then Return
            e.SuppressKeyPress = True ' منع الصوت المزعج

            ' 1. الحصول على القيم من الحقول
            Dim txtWillMaster As TextBox = CType(sender, TextBox)
            Dim masterPolicyNumber As String = txtWillMaster.Text.Trim()
            Dim txtWillSub As TextBox = GetControlByName(Of TextBox)(parentForm, "txtWillSub")
            Dim comboSubjBook As ComboBox = GetControlByName(Of ComboBox)(parentForm, "ComboSubjBook")

            ' 2. التحقق من الحقل الفارغ
            If String.IsNullOrEmpty(masterPolicyNumber) Then
                HandleEmptyMasterPolicy(parentForm, txtWillMaster, txtWillSub, comboSubjBook)
                Return
            End If

            ' 3. التحقق من وجود البوليصة في قاعدة البيانات
            If PolicyValidationHelper.CheckMasterPolicyExists(masterPolicyNumber) Then
                HandleExistingMasterPolicy(parentForm, masterPolicyNumber, txtWillSub, comboSubjBook)
            Else
                HandleNewMasterPolicy(parentForm, txtWillSub, comboSubjBook)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق من البوليصة الرئيسية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' حدث KeyDown للتحقق من البوليصة الفرعية عند الضغط على Enter
    ''' </summary>
    ''' <param name="sender">txtWillSub</param>
    ''' <param name="e">معاملات الحدث</param>
    ''' <param name="parentForm">النموذج الأب (SaderForm)</param>
    Public Sub txtWillSub_KeyDown(sender As Object, e As KeyEventArgs, parentForm As Form)
        Try
            If e.KeyCode <> Keys.Enter Then Return
            e.SuppressKeyPress = True ' منع الصوت المزعج

            ' 1. الحصول على القيم من الحقول
            Dim txtWillSub As TextBox = CType(sender, TextBox)
            Dim subPolicyNumber As String = txtWillSub.Text.Trim()
            Dim txtWillMaster As TextBox = GetControlByName(Of TextBox)(parentForm, "txtWillMaster")
            Dim comboSubjBook As ComboBox = GetControlByName(Of ComboBox)(parentForm, "ComboSubjBook")

            ' 2. التحقق من الحقل الفارغ
            If String.IsNullOrEmpty(subPolicyNumber) Then
                HandleEmptySubPolicy(parentForm, txtWillMaster, txtWillSub, comboSubjBook)
                Return
            End If

            ' 3. التحقق من وجود البوليصة في قاعدة البيانات
            If PolicyValidationHelper.CheckSubPolicyExists(subPolicyNumber) Then
                HandleExistingSubPolicy(parentForm, subPolicyNumber, txtWillMaster, comboSubjBook)
            Else
                HandleNewSubPolicy(parentForm, txtWillMaster, comboSubjBook)
            End If

        Catch ex As Exception
            MessageBox.Show($"خطأ في التحقق من البوليصة الفرعية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ========================
    ' الدوال المساعدة (منفصلة لتحسين القراءة)
    ' ========================

    ''' <summary>
    ''' التعامل مع البوليصة الرئيسية الفارغة
    ''' </summary>
    Private Sub HandleEmptyMasterPolicy(parentForm As Form,
                                   txtWillMaster As TextBox,
                                   txtWillSub As TextBox,
                                   cmbBookCategory As ComboBox)
        Dim askResult As DialogResult = MessageBox.Show(
        "هل توجد بوليصة رئيسية؟",
        "استفسار",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If askResult = DialogResult.Yes Then
            FocusOnControl(txtWillMaster)
        Else
            ' التحقق من وجود قيمة في البوليصة الفرعية
            Dim subPolicyValue As String = If(txtWillSub IsNot Nothing, txtWillSub.Text.Trim(), "")

            ' إذا كانت البوليصة الفرعية تحتوي على قيمة، تخطى السؤال وانتقل مباشرة إلى ComboSubjBook
            If Not String.IsNullOrEmpty(subPolicyValue) Then
                FocusOnControl(cmbBookCategory)
            Else
                ' السؤال عن البوليصة الفرعية فقط إذا كانت فارغة
                Dim subAskResult As DialogResult = MessageBox.Show(
                "هل توجد بوليصة فرعية؟",
                "استفسار",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If subAskResult = DialogResult.Yes Then
                    FocusOnControl(txtWillSub)
                Else
                    FocusOnControl(cmbBookCategory)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' التعامل مع البوليصة الرئيسية الموجودة
    ''' </summary>
    Private Sub HandleExistingMasterPolicy(parentForm As Form,
                                      masterPolicyNumber As String,
                                      txtWillSub As TextBox,
                                      cmbBookCategory As ComboBox)
        Dim result As DialogResult = MessageBox.Show(
        $"البوليصة الرئيسية رقم {vbCrLf}{vbCrLf} ( {masterPolicyNumber} ) {vbCrLf} {vbCrLf} موجودة مسبقاً. {vbCrLf} هل ترغب في المواصلة؟",
        "تأكيد",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2)

        If result = DialogResult.No Then
            ' لا يرغب في المواصلة - البقاء في الحقل
            FocusOnControl(CType(parentForm.ActiveControl, TextBox))
        Else
            ' التحقق من وجود قيمة في البوليصة الفرعية
            Dim subPolicyValue As String = If(txtWillSub IsNot Nothing, txtWillSub.Text.Trim(), "")

            ' إذا كانت البوليصة الفرعية تحتوي على قيمة، تخطى السؤال وانتقل مباشرة إلى ComboSubjBook
            If Not String.IsNullOrEmpty(subPolicyValue) Then
                FocusOnControl(cmbBookCategory)
            Else
                ' السؤال عن البوليصة الفرعية فقط إذا كانت فارغة
                Dim subResult As DialogResult = MessageBox.Show(
                "هل توجد بوليصة فرعية؟",
                "استفسار",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If subResult = DialogResult.Yes Then
                    FocusOnControl(txtWillSub)
                Else
                    FocusOnControl(cmbBookCategory)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' التعامل مع البوليصة الرئيسية الجديدة
    ''' </summary>
    Private Sub HandleNewMasterPolicy(parentForm As Form,
                                 txtWillSub As TextBox,
                                 cmbBookCategory As ComboBox)
        ' التحقق من وجود قيمة في البوليصة الفرعية
        Dim subPolicyValue As String = If(txtWillSub IsNot Nothing, txtWillSub.Text.Trim(), "")

        ' إذا كانت البوليصة الفرعية تحتوي على قيمة، تخطى السؤال وانتقل مباشرة إلى ComboSubjBook
        If Not String.IsNullOrEmpty(subPolicyValue) Then
            FocusOnControl(cmbBookCategory)
        Else
            ' السؤال عن البوليصة الفرعية فقط إذا كانت فارغة
            Dim subResult As DialogResult = MessageBox.Show(
            "هل توجد بوليصة فرعية؟",
            "استفسار",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

            If subResult = DialogResult.Yes Then
                FocusOnControl(txtWillSub)
            Else
                FocusOnControl(cmbBookCategory)
            End If
        End If
    End Sub

    ' هذه الدوال يجب إضافتها إلى PolicyValidationEvents.vb
    ' لكن سأضع التعديلات هنا للإشارة إلى التغيير المطلوب

    ''' <summary>
    ''' التعامل مع البوليصة الفرعية الفارغة
    ''' </summary>
    Private Sub HandleEmptySubPolicy(parentForm As Form,
                                txtWillMaster As TextBox,
                                txtWillSub As TextBox,
                                cmbBookCategory As ComboBox)
        Dim askResult As DialogResult = MessageBox.Show(
        "هل توجد بوليصة فرعية؟",
        "استفسار",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If askResult = DialogResult.Yes Then
            FocusOnControl(txtWillSub)
        Else
            ' التحقق من وجود قيمة في البوليصة الرئيسية
            Dim masterPolicyValue As String = If(txtWillMaster IsNot Nothing, txtWillMaster.Text.Trim(), "")

            ' إذا كانت البوليصة الرئيسية تحتوي على قيمة، تخطى السؤال وانتقل مباشرة إلى ComboSubjBook
            If Not String.IsNullOrEmpty(masterPolicyValue) Then
                FocusOnControl(cmbBookCategory)
            Else
                ' السؤال عن البوليصة الرئيسية فقط إذا كانت فارغة
                Dim masterAskResult As DialogResult = MessageBox.Show(
                "هل توجد بوليصة رئيسية؟",
                "استفسار",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If masterAskResult = DialogResult.Yes Then
                    FocusOnControl(txtWillMaster)
                Else
                    FocusOnControl(cmbBookCategory)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' التعامل مع البوليصة الفرعية الموجودة
    ''' </summary>
    Private Sub HandleExistingSubPolicy(parentForm As Form,
                                   subPolicyNumber As String,
                                   txtWillMaster As TextBox,
                                   cmbBookCategory As ComboBox)
        Dim result As DialogResult = MessageBox.Show(
        $"البوليصة الفرعية رقم {vbCrLf}{vbCrLf} ( {subPolicyNumber} ) {vbCrLf} {vbCrLf} موجودة مسبقاً. {vbCrLf} هل ترغب في المواصلة؟",
        "تأكيد",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question,
        MessageBoxDefaultButton.Button2)

        If result = DialogResult.No Then
            ' لا يرغب في المواصلة - البقاء في الحقل
            FocusOnControl(CType(parentForm.ActiveControl, TextBox))
        Else
            ' التحقق من وجود قيمة في البوليصة الرئيسية
            Dim masterPolicyValue As String = If(txtWillMaster IsNot Nothing, txtWillMaster.Text.Trim(), "")

            ' إذا كانت البوليصة الرئيسية تحتوي على قيمة، تخطى السؤال وانتقل مباشرة إلى ComboSubjBook
            If Not String.IsNullOrEmpty(masterPolicyValue) Then
                FocusOnControl(cmbBookCategory)
            Else
                ' السؤال عن البوليصة الرئيسية فقط إذا كانت فارغة
                Dim masterResult As DialogResult = MessageBox.Show(
                "هل توجد بوليصة رئيسية؟",
                "استفسار",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

                If masterResult = DialogResult.Yes Then
                    FocusOnControl(txtWillMaster)
                Else
                    FocusOnControl(cmbBookCategory)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' التعامل مع البوليصة الفرعية الجديدة
    ''' </summary>
    Private Sub HandleNewSubPolicy(parentForm As Form,
                              txtWillMaster As TextBox,
                              cmbBookCategory As ComboBox)
        ' التحقق من وجود قيمة في البوليصة الرئيسية
        Dim masterPolicyValue As String = If(txtWillMaster IsNot Nothing, txtWillMaster.Text.Trim(), "")

        ' إذا كانت البوليصة الرئيسية تحتوي على قيمة، تخطى السؤال وانتقل مباشرة إلى ComboSubjBook
        If Not String.IsNullOrEmpty(masterPolicyValue) Then
            FocusOnControl(cmbBookCategory)
        Else
            ' السؤال عن البوليصة الرئيسية فقط إذا كانت فارغة
            Dim masterResult As DialogResult = MessageBox.Show(
            "هل توجد بوليصة رئيسية؟",
            "استفسار",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

            If masterResult = DialogResult.Yes Then
                FocusOnControl(txtWillMaster)
            Else
                FocusOnControl(cmbBookCategory)
            End If
        End If
    End Sub

    ' ========================
    ' الدوال العامة المفيدة
    ' ========================

    ''' <summary>
    ''' الحصول على عنصر تحكم من النموذج
    ''' </summary>
    Private Function GetControlByName(Of T As Control)(parentForm As Form, controlName As String) As T
        Return TryCast(parentForm.Controls.Find(controlName, True).FirstOrDefault(), T)
    End Function

    ''' <summary>
    ''' التركيز على عنصر تحكم مع تحديد المحتوى
    ''' </summary>
    Private Sub FocusOnControl(control As Control)
        If control Is Nothing Then Return

        control.Focus()

        ' تحديد المحتوى إذا كان TextBox
        If TypeOf control Is TextBox Then
            CType(control, TextBox).SelectAll()
        ElseIf TypeOf control Is ComboBox Then
            CType(control, ComboBox).DroppedDown = True
        End If
    End Sub

    ' ========================
    ' إدارة الأحداث (كما هي مع تحسينات بسيطة)
    ' ========================

    ' متغيرات خاصة لتخزين معالجات الأحداث

    ''' <summary>
    ''' ربط أحداث التحقق من البوليصات
    ''' </summary>
    Public Sub AttachPolicyValidationEvents(parentForm As Form)
        Try
            ' إنشاء المعالجات
            _willMasterKeyDownHandler = Sub(sender, e) txtWillMaster_KeyDown(sender, e, parentForm)
            _willSubKeyDownHandler = Sub(sender, e) txtWillSub_KeyDown(sender, e, parentForm)

            ' ربط الأحداث
            AttachEvent(parentForm, "txtWillMaster", _willMasterKeyDownHandler)
            AttachEvent(parentForm, "txtWillSub", _willSubKeyDownHandler)

        Catch ex As Exception
            Debug.WriteLine($"خطأ في ربط الأحداث: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' مساعدة لربط الحدث
    ''' </summary>
    Private Sub AttachEvent(parentForm As Form, controlName As String, handler As KeyEventHandler)
        Dim control As Control = GetControlByName(Of Control)(parentForm, controlName)
        If control IsNot Nothing Then
            AddHandler CType(control, TextBox).KeyDown, handler
        End If
    End Sub

    ''' <summary>
    ''' إلغاء ربط أحداث التحقق من البوليصات
    ''' </summary>
    Public Sub DetachPolicyValidationEvents(parentForm As Form)
        Try
            ' إلغاء ربط الأحداث
            DetachEvent(parentForm, "txtWillMaster", _willMasterKeyDownHandler)
            DetachEvent(parentForm, "txtWillSub", _willSubKeyDownHandler)

            ' تنظيف المتغيرات
            _willMasterKeyDownHandler = Nothing
            _willSubKeyDownHandler = Nothing

        Catch ex As Exception
            Debug.WriteLine($"خطأ في إلغاء الربط: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' مساعدة لإلغاء ربط الحدث
    ''' </summary>
    Private Sub DetachEvent(parentForm As Form, controlName As String, handler As KeyEventHandler)
        If handler Is Nothing Then Return

        Dim control As Control = GetControlByName(Of Control)(parentForm, controlName)
        If control IsNot Nothing Then
            RemoveHandler CType(control, TextBox).KeyDown, handler
        End If
    End Sub

End Module




'''' <summary>
'''' أحداث التحقق من البوليصات للاستخدام في SaderForm - نسخة محدثة مع السؤال عن الحقول الفارغة
'''' </summary>
'Public Module PolicyValidationEvents


'    Private txtWillSub As TextBox = CType(sender, TextBox)
'    Private subPolicyNumber As String = txtWillSub.Text.Trim()

'    Private txtWillMaster As TextBox = CType(sender, TextBox)
'    Private masterPolicyNumber As String = txtWillMaster.Text.Trim()

'    ''' <summary>
'    ''' حدث KeyDown للتحقق من البوليصة الرئيسية عند الضغط على Enter
'    ''' </summary>
'    ''' <param name="sender">txtWillMaster</param>
'    ''' <param name="e">معاملات الحدث</param>
'    ''' <param name="parentForm">النموذج الأب (SaderForm)</param>
'    Public Sub txtWillMaster_KeyDown(sender As Object, e As KeyEventArgs, parentForm As Form)
'        Try
'            If e.KeyCode = Keys.Enter Then
'                e.SuppressKeyPress = True ' منع الصوت المزعج

'                Dim txtWillMaster As TextBox = CType(sender, TextBox)
'                Dim masterPolicyNumber As String = txtWillMaster.Text.Trim()

'                ' إذا كان الحقل فارغاً، اسأل المستخدم
'                If String.IsNullOrEmpty(masterPolicyNumber) Then
'                    Dim askMessage As String = "هل توجد بوليصة رئيسية؟"
'                    Dim askResult As DialogResult = MessageBox.Show(askMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                    If askResult = DialogResult.Yes Then
'                        ' المستخدم اختار "نعم" - البقاء في نفس الحقل للإدخال
'                        FocusOnTxtWillMaster(parentForm)
'                        Return
'                    Else
'                        ' المستخدم اختار "لا" - السؤال عن البوليصة الفرعية
'                        Dim subAskMessage As String = "هل توجد بوليصة فرعية؟"
'                        Dim subAskResult As DialogResult = MessageBox.Show(subAskMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                        If subAskResult = DialogResult.Yes Then
'                            ' يوجد بوليصة فرعية - التركيز على txtWillSub
'                            FocusOnTxtWillSub(parentForm)
'                        Else
'                            ' لا توجد بوليصة فرعية - التركيز على ComboSubjBook
'                            FocusOnComboSubjBook(parentForm)
'                        End If
'                        Return
'                    End If
'                End If

'                ' إذا كان الحقل يحتوي على قيمة، تحقق من وجودها في قاعدة البيانات
'                If PolicyValidationHelper.CheckMasterPolicyExists(masterPolicyNumber) Then
'                    ' البوليصة موجودة - عرض رسالة تأكيد
'                    Dim message As String = $"البوليصة الرئيسية رقم {masterPolicyNumber} موجودة مسبقاً.{vbCrLf}هل ترغب في المواصلة؟"
'                    Dim result As DialogResult = MessageBox.Show(message, "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

'                    If result = DialogResult.No Then
'                        ' المستخدم اختار "لا" - التركيز على ComboSubjBook
'                        FocusOnTxtWillMaster(parentForm)
'                    Else
'                        ' المستخدم اختار "نعم" - سؤال عن البوليصة الفرعية
'                        Dim subMessage As String = "هل توجد بوليصة فرعية؟"
'                        Dim subResult As DialogResult = MessageBox.Show(subMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                        If subResult = DialogResult.Yes Then
'                            ' يوجد بوليصة فرعية - التركيز على txtWillSub
'                            FocusOnTxtWillSub(parentForm)
'                        Else
'                            ' لا توجد بوليصة فرعية - التركيز على ComboSubjBook
'                            FocusOnComboSubjBook(parentForm)
'                        End If
'                    End If
'                Else
'                    ' البوليصة غير موجودة - سؤال عن البوليصة الفرعية
'                    Dim subMessage As String = "هل توجد بوليصة فرعية؟"
'                    Dim subResult As DialogResult = MessageBox.Show(subMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                    If subResult = DialogResult.Yes Then
'                        ' يوجد بوليصة فرعية - التركيز على txtWillSub
'                        FocusOnTxtWillSub(parentForm)
'                    Else
'                        ' لا توجد بوليصة فرعية - التركيز على ComboSubjBook
'                        FocusOnComboSubjBook(parentForm)
'                    End If
'                End If
'            End If

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في التحقق من البوليصة الرئيسية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' حدث KeyDown للتحقق من البوليصة الفرعية عند الضغط على Enter
'    ''' </summary>
'    ''' <param name="sender">txtWillSub</param>
'    ''' <param name="e">معاملات الحدث</param>
'    ''' <param name="parentForm">النموذج الأب (SaderForm)</param>
'    Public Sub txtWillSub_KeyDown(sender As Object, e As KeyEventArgs, parentForm As Form)
'        Try
'            If e.KeyCode = Keys.Enter Then
'                e.SuppressKeyPress = True ' منع الصوت المزعج

'                'Dim txtWillSub As TextBox = CType(sender, TextBox)
'                'Dim subPolicyNumber As String = txtWillSub.Text.Trim()

'                ' إذا كان الحقل فارغاً، اسأل المستخدم
'                If String.IsNullOrEmpty(subPolicyNumber) Then
'                    Dim askMessage As String = "هل توجد بوليصة فرعية؟"
'                    Dim askResult As DialogResult = MessageBox.Show(askMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                    If askResult = DialogResult.Yes Then
'                        ' المستخدم اختار "نعم" - البقاء في نفس الحقل للإدخال
'                        FocusOnTxtWillSub(parentForm)
'                        Return
'                    Else
'                        ' المستخدم اختار "لا" - السؤال عن البوليصة الرئيسية
'                        Dim masterAskMessage As String = "هل توجد بوليصة رئيسية؟"
'                        Dim masterAskResult As DialogResult = MessageBox.Show(masterAskMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                        If masterAskResult = DialogResult.Yes Then
'                            ' يوجد بوليصة رئيسية - التركيز على txtWillMaster
'                            FocusOnTxtWillMaster(parentForm)
'                        Else
'                            ' لا توجد بوليصة رئيسية - التركيز على ComboSubjBook
'                            FocusOnComboSubjBook(parentForm)
'                        End If
'                        Return
'                    End If
'                End If

'                ' إذا كان الحقل يحتوي على قيمة، تحقق من وجودها في قاعدة البيانات
'                If PolicyValidationHelper.CheckSubPolicyExists(subPolicyNumber) Then
'                    ' البوليصة موجودة - عرض رسالة تأكيد
'                    Dim message As String = $"البوليصة الفرعية رقم {subPolicyNumber} موجودة مسبقاً.{vbCrLf}هل ترغب في المواصلة؟"
'                    Dim result As DialogResult = MessageBox.Show(message, "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

'                    If result = DialogResult.No Then
'                        ' المستخدم اختار "لا" - التركيز على ComboSubjBook
'                        FocusOnTxtWillSub(parentForm)
'                    Else
'                        If String.IsNullOrEmpty(subPolicyNumber) Then

'                        End If
'                        ' المستخدم اختار "نعم" - سؤال عن البوليصة الرئيسية
'                        Dim masterMessage As String = "هل توجد بوليصة رئيسية؟"
'                        Dim masterResult As DialogResult = MessageBox.Show(masterMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                        If masterResult = DialogResult.Yes Then
'                            ' يوجد بوليصة رئيسية - التركيز على txtWillMaster
'                            FocusOnTxtWillMaster(parentForm)
'                        Else
'                            ' لا توجد بوليصة رئيسية - التركيز على ComboSubjBook
'                            FocusOnComboSubjBook(parentForm)
'                        End If
'                    End If
'                ElseIf String.IsNullOrEmpty(txtWillMaster) Then
'                    ' البوليصة غير موجودة - سؤال عن البوليصة الرئيسية
'                    Dim masterMessage As String = "هل توجد بوليصة رئيسية؟"
'                    Dim masterResult As DialogResult = MessageBox.Show(masterMessage, "استفسار", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

'                    If masterResult = DialogResult.Yes Then
'                        ' يوجد بوليصة رئيسية - التركيز على txtWillMaster
'                        FocusOnTxtWillMaster(parentForm)
'                    Else
'                        ' لا توجد بوليصة رئيسية - التركيز على ComboSubjBook
'                        FocusOnComboSubjBook(parentForm)
'                    End If
'                End If
'            End If

'        Catch ex As Exception
'            MessageBox.Show($"خطأ في التحقق من البوليصة الفرعية: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub

'    ''' <summary>
'    ''' التركيز على txtWillMaster
'    ''' </summary>
'    Private Sub FocusOnTxtWillMaster(parentForm As Form)
'        Try
'            Dim txtWillMaster As TextBox = TryCast(parentForm.Controls.Find("txtWillMaster", True).FirstOrDefault(), TextBox)
'            If txtWillMaster IsNot Nothing Then
'                txtWillMaster.Focus()
'                txtWillMaster.SelectAll()
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التركيز على txtWillMaster: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' التركيز على txtWillSub
'    ''' </summary>
'    Private Sub FocusOnTxtWillSub(parentForm As Form)
'        Try
'            Dim txtWillSub As TextBox = TryCast(parentForm.Controls.Find("txtWillSub", True).FirstOrDefault(), TextBox)
'            If txtWillSub IsNot Nothing Then
'                txtWillSub.Focus()
'                txtWillSub.SelectAll()
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التركيز على txtWillSub: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' التركيز على ComboSubjBook
'    ''' </summary>
'    Private Sub FocusOnComboSubjBook(parentForm As Form)
'        Try
'            Dim comboSubjBook As ComboBox = TryCast(parentForm.Controls.Find("ComboSubjBook", True).FirstOrDefault(), ComboBox)
'            If comboSubjBook IsNot Nothing Then
'                comboSubjBook.DroppedDown = True
'                comboSubjBook.Focus()

'                ' تأخير بسيط لضمان عمل الأسهم
'                Application.DoEvents()
'                System.Threading.Thread.Sleep(50)
'            End If
'        Catch ex As Exception
'            System.Diagnostics.Debug.WriteLine($"خطأ في التركيز على ComboSubjBook: {ex.Message}")
'        End Try
'    End Sub

'    ' متغيرات خاصة لتخزين معالجات الأحداث
'    Private _willMasterKeyDownHandler As KeyEventHandler
'    Private _willSubKeyDownHandler As KeyEventHandler

'    ''' <summary>
'    ''' ربط أحداث التحقق من البوليصات (إضافة جديدة)
'    ''' </summary>
'    Public Sub AttachPolicyValidationEvents(parentForm As Form)
'        Try
'            Dim txtWillMaster = TryCast(parentForm.Controls.Find("txtWillMaster", True).FirstOrDefault(), TextBox)
'            Dim txtWillSub = TryCast(parentForm.Controls.Find("txtWillSub", True).FirstOrDefault(), TextBox)

'            ' إنشاء المعالجات وتخزينها في المتغيرات
'            _willMasterKeyDownHandler = Sub(sender, e) txtWillMaster_KeyDown(sender, e, parentForm)
'            _willSubKeyDownHandler = Sub(sender, e) txtWillSub_KeyDown(sender, e, parentForm)

'            ' الربط باستخدام المتغيرات
'            If txtWillMaster IsNot Nothing Then
'                AddHandler txtWillMaster.KeyDown, _willMasterKeyDownHandler
'            End If

'            If txtWillSub IsNot Nothing Then
'                AddHandler txtWillSub.KeyDown, _willSubKeyDownHandler
'            End If

'        Catch ex As Exception
'            Debug.WriteLine($"خطأ في ربط الأحداث: {ex.Message}")
'        End Try
'    End Sub

'    ''' <summary>
'    ''' إلغاء ربط أحداث التحقق من البوليصات
'    ''' </summary>
'    Public Sub DetachPolicyValidationEvents(parentForm As Form)
'        Try
'            Dim txtWillMaster = TryCast(parentForm.Controls.Find("txtWillMaster", True).FirstOrDefault(), TextBox)
'            Dim txtWillSub = TryCast(parentForm.Controls.Find("txtWillSub", True).FirstOrDefault(), TextBox)

'            ' الإلغاء باستخدام نفس المتغيرات
'            If txtWillMaster IsNot Nothing AndAlso _willMasterKeyDownHandler IsNot Nothing Then
'                RemoveHandler txtWillMaster.KeyDown, _willMasterKeyDownHandler
'            End If

'            If txtWillSub IsNot Nothing AndAlso _willSubKeyDownHandler IsNot Nothing Then
'                RemoveHandler txtWillSub.KeyDown, _willSubKeyDownHandler
'            End If

'            ' تنظيف المتغيرات (اختياري)
'            _willMasterKeyDownHandler = Nothing
'            _willSubKeyDownHandler = Nothing

'        Catch ex As Exception
'            Debug.WriteLine($"خطأ في إلغاء الربط: {ex.Message}")
'        End Try
'    End Sub



'    '''' <summary>
'    '''' ربط أحداث التحقق من البوليصات بالنموذج
'    '''' </summary>
'    '''' <param name="parentForm">النموذج الأب (SaderForm)</param>
'    'Public Sub AttachPolicyValidationEvents(parentForm As Form)
'    '    Try
'    '        ' البحث عن عناصر التحكم
'    '        Dim txtWillMaster As TextBox = TryCast(parentForm.Controls.Find("txtWillMaster", True).FirstOrDefault(), TextBox)
'    '        Dim txtWillSub As TextBox = TryCast(parentForm.Controls.Find("txtWillSub", True).FirstOrDefault(), TextBox)

'    '        ' ربط الأحداث
'    '        If txtWillMaster IsNot Nothing Then
'    '            AddHandler txtWillMaster.KeyDown, Sub(sender, e) txtWillMaster_KeyDown(sender, e, parentForm)
'    '        End If

'    '        If txtWillSub IsNot Nothing Then
'    '            AddHandler txtWillSub.KeyDown, Sub(sender, e) txtWillSub_KeyDown(sender, e, parentForm)
'    '        End If

'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في ربط أحداث التحقق من البوليصات: {ex.Message}")
'    '    End Try
'    'End Sub




'    '''' <summary>
'    '''' إلغاء ربط أحداث التحقق من البوليصات
'    '''' </summary>
'    '''' <param name="parentForm">النموذج الأب (SaderForm)</param>
'    'Public Sub DetachPolicyValidationEvents(parentForm As Form)
'    '    Try
'    '        ' البحث عن عناصر التحكم
'    '        Dim txtWillMaster As TextBox = TryCast(parentForm.Controls.Find("txtWillMaster", True).FirstOrDefault(), TextBox)
'    '        Dim txtWillSub As TextBox = TryCast(parentForm.Controls.Find("txtWillSub", True).FirstOrDefault(), TextBox)

'    '        ' إلغاء ربط الأحداث
'    '        If txtWillMaster IsNot Nothing Then
'    '            RemoveHandler txtWillMaster.KeyDown, Sub(sender, e) txtWillMaster_KeyDown(sender, e, parentForm)
'    '        End If

'    '        If txtWillSub IsNot Nothing Then
'    '            RemoveHandler txtWillSub.KeyDown, Sub(sender, e) txtWillSub_KeyDown(sender, e, parentForm)
'    '        End If

'    '    Catch ex As Exception
'    '        System.Diagnostics.Debug.WriteLine($"خطأ في إلغاء ربط أحداث التحقق من البوليصات: {ex.Message}")
'    '    End Try
'    'End Sub

'End Module
