Imports System.IO

Public Class ManageCategoriesForm

    Private categories As List(Of String) = New List(Of String)()

    Private Sub ManageCategoriesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCategories()
    End Sub

    ''' <summary>
    ''' تحميل التصنيفات من الملف
    ''' </summary>

    Private Sub LoadCategories()
        Try
            Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

            If Not File.Exists(filePath) Then
                Dim defaultCategories As String() = {"شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}
                File.WriteAllLines(filePath, defaultCategories)
            End If

            categories = File.ReadAllLines(filePath) _
            .Where(Function(c) Not String.IsNullOrWhiteSpace(c)) _
            .Select(Function(c) c.Trim()) _
            .OrderBy(Function(c) c) _   ' 🔹 ترتيب أبجدي
            .ToList()

            RefreshListBox(categories)

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تحميل تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    'Private Sub LoadCategories()
    '    Try
    '        Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

    '        ' التحقق من وجود الملف
    '        If Not File.Exists(filePath) Then
    '            ' إذا لم يكن الملف موجودًا، قم بإنشائه مع القيم الافتراضية
    '            Dim defaultCategories As String() = {"شركات اخرى", "الجمارك العامة", "مذكرات", "تصدير", "إفراج جمركي"}
    '            File.WriteAllLines(filePath, defaultCategories)
    '        End If

    '        ' قراءة التصنيفات من الملف
    '        categories = File.ReadAllLines(filePath).Where(Function(c) Not String.IsNullOrWhiteSpace(c)).Select(Function(c) c.Trim()).ToList()

    '        ' عرض التصنيفات في ListBox
    '        lstCategories.Items.Clear()
    '        For Each category In categories
    '            lstCategories.Items.Add(category)
    '        Next

    '    Catch ex As Exception
    '        MessageBox.Show("حدث خطأ أثناء تحميل تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub RefreshListBox(list As List(Of String))
        lstCategories.Items.Clear()
        For Each c In list
            lstCategories.Items.Add(c)
        Next
    End Sub


    ''' <summary>
    ''' إضافة تصنيف جديد
    ''' </summary>
    Private Sub btnAddCategory_Click(sender As Object, e As EventArgs) Handles btnAddCategory.Click
        Try
            Dim newCategory As String = txtNewCategory.Text.Trim()

            ' التحقق من أن التصنيف ليس فارغًا
            If String.IsNullOrWhiteSpace(newCategory) Then
                MessageBox.Show("يرجى إدخال اسم التصنيف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من أن التصنيف غير مكرر
            If categories.Contains(newCategory) Then
                MessageBox.Show("هذا التصنيف موجود بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If


            categories.Add(newCategory)
            categories = categories.OrderBy(Function(c) c).ToList()
            RefreshListBox(categories)


            '' إضافة التصنيف إلى القائمة
            'categories.Add(newCategory)

            '' تحديث ListBox
            'lstCategories.Items.Add(newCategory)

            ' مسح حقل الإدخال
            txtNewCategory.Clear()
            txtNewCategory.Focus()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء إضافة التصنيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' حفظ التصنيفات في الملف
    ''' </summary>
    Private Sub btnSaveCategories_Click(sender As Object, e As EventArgs) Handles btnSaveCategories.Click
        Try
            Dim filePath As String = Path.Combine(Application.StartupPath, "BookCategories.txt")

            ' حفظ التصنيفات في الملف
            File.WriteAllLines(filePath, categories)

            MessageBox.Show($"تم حفظ التصنيف '{txtNewCategory.Text}' بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حفظ تصنيفات الكتاب: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' حذف تصنيف محدد
    ''' </summary>
    Private Sub btnDeleteCategory_Click(sender As Object, e As EventArgs) Handles btnDeleteCategory.Click
        Try
            ' التحقق من أن عنصرًا محددًا
            If lstCategories.SelectedIndex >= 0 Then
                Dim selectedCategory As String = lstCategories.SelectedItem.ToString()

                ' حذف التصنيف من القائمة
                categories.Remove(selectedCategory)

                ' تحديث ListBox
                lstCategories.Items.RemoveAt(lstCategories.SelectedIndex)

                categories = categories.OrderBy(Function(c) c).ToList()
                RefreshListBox(categories)


                MessageBox.Show($"تم حذف التصنيف '{selectedCategory}' بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("يرجى اختيار تصنيف للحذف", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء حذف التصنيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lstCategories_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstCategories.SelectedIndexChanged
        If lstCategories.SelectedIndex >= 0 Then
            txtNewCategory.Text = lstCategories.SelectedItem.ToString()
        End If
    End Sub

    Private Sub btnEditCategory_Click(sender As Object, e As EventArgs) Handles btnEditCategory.Click
        Try
            ' التأكد من وجود عنصر محدد
            If lstCategories.SelectedIndex < 0 Then
                MessageBox.Show("يرجى اختيار تصنيف لتعديله", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim newName As String = txtNewCategory.Text.Trim()

            ' التحقق من أن الاسم ليس فارغًا
            If String.IsNullOrWhiteSpace(newName) Then
                MessageBox.Show("يرجى إدخال اسم التصنيف الجديد", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' التحقق من عدم التكرار (مع استثناء العنصر الحالي)
            Dim oldName As String = lstCategories.SelectedItem.ToString()
            If categories.Any(Function(c) c.Equals(newName, StringComparison.OrdinalIgnoreCase)) AndAlso newName <> oldName Then
                MessageBox.Show("هذا التصنيف موجود بالفعل", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim index As Integer = lstCategories.SelectedIndex

            ' تحديث القائمة الداخلية
            categories(index) = newName

            ' تحديث ListBox
            lstCategories.Items(index) = newName

            MessageBox.Show("تم تعديل التصنيف بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information)

            txtNewCategory.Clear()

        Catch ex As Exception
            MessageBox.Show("حدث خطأ أثناء تعديل التصنيف: " & ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtSearchCategory_TextChanged(sender As Object, e As EventArgs) Handles txtSearchCategory.TextChanged
        Dim keyword As String = txtSearchCategory.Text.Trim()

        If String.IsNullOrEmpty(keyword) Then
            ' عرض الكل مع الترتيب الأبجدي
            RefreshListBox(categories.OrderBy(Function(c) c).ToList())
        Else
            ' 🔹 بحث غير حساس لحالة الأحرف
            Dim filtered = categories.
            Where(Function(c) c.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).
            OrderBy(Function(c) c).ToList()


            RefreshListBox(filtered)
        End If
    End Sub


End Class