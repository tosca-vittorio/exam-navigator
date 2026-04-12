using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ExamNavigator.Application.Contracts;
using ExamNavigator.Application.Services;

namespace ExamNavigator.WinForms
{
    public partial class Form1 : Form
    {
        private readonly IExamNavigationService _navigationService;
        private bool _isLoadingNavigation;

        public Form1()
            : this(new BootstrapNavigationService())
        {
        }

        public Form1(IExamNavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            InitializeComponent();
            ConfigureExamListPresentation();
            WireEvents();
        }

        private sealed class SearchFieldOption
        {
            public ExamSearchField Value { get; set; }

            public string Label { get; set; } = string.Empty;
        }

        private sealed class ExamDisplayItem
        {
            public int Id { get; set; }

            public string DisplayLabel { get; set; } = string.Empty;

            public ExamListItem Exam { get; set; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeSearchField();
            LoadNavigation(new ExamNavigationRequest
            {
                SearchText = txtSearchTerm.Text,
                SearchField = GetSelectedSearchField()
            });
        }

        private void ConfigureExamListPresentation()
        {
            lstExams.DrawMode = DrawMode.OwnerDrawVariable;
            lstExams.HorizontalScrollbar = false;
            lstExams.IntegralHeight = false;
            lstExams.DrawItem += LstExams_DrawItem;
            lstExams.MeasureItem += LstExams_MeasureItem;
        }

        private void WireEvents()
        {
            lstRooms.SelectedIndexChanged += LstRooms_SelectedIndexChanged;
            lstBodyParts.SelectedIndexChanged += LstBodyParts_SelectedIndexChanged;
            btnConfirmExam.Click += BtnConfirmExam_Click;
            btnRemoveSelected.Click += BtnRemoveSelected_Click;
            btnMoveUp.Click += BtnMoveUp_Click;
            btnMoveDown.Click += BtnMoveDown_Click;
            btnSearch.Click += BtnSearch_Click;
            btnClearSearch.Click += BtnClearSearch_Click;
            txtSearchTerm.KeyDown += TxtSearchTerm_KeyDown;
        }

        private void LstRooms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingNavigation)
            {
                return;
            }

            LoadNavigation(new ExamNavigationRequest
            {
                SelectedRoomId = GetSelectedLookupId(lstRooms),
                SearchText = txtSearchTerm.Text,
                SearchField = GetSelectedSearchField()
            });
        }

        private void LstBodyParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoadingNavigation)
            {
                return;
            }

            LoadNavigation(new ExamNavigationRequest
            {
                SelectedRoomId = GetSelectedLookupId(lstRooms),
                SelectedBodyPartId = GetSelectedLookupId(lstBodyParts),
                SearchText = txtSearchTerm.Text,
                SearchField = GetSelectedSearchField()
            });
        }

        private void BtnConfirmExam_Click(object sender, EventArgs e)
        {
            var selectedExam = GetSelectedExamItem();
            var selectedBodyPart = lstBodyParts.SelectedItem as LookupItem;
            var selectedRoom = lstRooms.SelectedItem as LookupItem;

            if (selectedExam == null || selectedBodyPart == null || selectedRoom == null)
            {
                return;
            }

            dgvSelectedExams.Rows.Add(
                selectedExam.CodiceMinisteriale,
                selectedExam.CodiceInterno,
                selectedExam.DescrizioneEsame,
                selectedBodyPart.Label,
                selectedRoom.Label);
        }

        private void BtnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (dgvSelectedExams.SelectedRows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow selectedRow in dgvSelectedExams.SelectedRows)
            {
                if (!selectedRow.IsNewRow)
                {
                    dgvSelectedExams.Rows.Remove(selectedRow);
                }
            }
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (dgvSelectedExams.SelectedRows.Count == 0)
            {
                return;
            }

            var selectedRow = dgvSelectedExams.SelectedRows[0];
            if (selectedRow.IsNewRow)
            {
                return;
            }

            var selectedIndex = selectedRow.Index;
            if (selectedIndex <= 0)
            {
                return;
            }

            var targetIndex = selectedIndex - 1;
            var values = new object[selectedRow.Cells.Count];

            for (var i = 0; i < selectedRow.Cells.Count; i++)
            {
                values[i] = selectedRow.Cells[i].Value;
            }

            dgvSelectedExams.Rows.RemoveAt(selectedIndex);
            dgvSelectedExams.Rows.Insert(targetIndex, values);
            dgvSelectedExams.ClearSelection();
            dgvSelectedExams.Rows[targetIndex].Selected = true;
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (dgvSelectedExams.SelectedRows.Count == 0)
            {
                return;
            }

            var selectedRow = dgvSelectedExams.SelectedRows[0];
            if (selectedRow.IsNewRow)
            {
                return;
            }

            var selectedIndex = selectedRow.Index;
            if (selectedIndex < 0 || selectedIndex >= dgvSelectedExams.Rows.Count - 1)
            {
                return;
            }

            var targetIndex = selectedIndex + 1;
            var values = new object[selectedRow.Cells.Count];

            for (var i = 0; i < selectedRow.Cells.Count; i++)
            {
                values[i] = selectedRow.Cells[i].Value;
            }

            dgvSelectedExams.Rows.RemoveAt(selectedIndex);
            dgvSelectedExams.Rows.Insert(targetIndex, values);
            dgvSelectedExams.ClearSelection();
            dgvSelectedExams.Rows[targetIndex].Selected = true;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplySearch();
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchTerm.Text = string.Empty;
            ApplySearch();
        }

        private void TxtSearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            ApplySearch();
        }

        private void ApplySearch()
        {
            LoadNavigation(new ExamNavigationRequest
            {
                SelectedRoomId = GetSelectedLookupId(lstRooms),
                SelectedBodyPartId = GetSelectedLookupId(lstBodyParts),
                SearchText = txtSearchTerm.Text,
                SearchField = GetSelectedSearchField()
            });
        }

        private void InitializeSearchField()
        {
            txtSearchTerm.Text = Predefiniti_Ricerca.SearchText ?? string.Empty;

            var searchFieldOptions = BuildSearchFieldOptions();

            cmbSearchField.DataSource = null;
            cmbSearchField.DisplayMember = nameof(SearchFieldOption.Label);
            cmbSearchField.ValueMember = nameof(SearchFieldOption.Value);
            cmbSearchField.DataSource = searchFieldOptions;

            cmbSearchField.SelectedValue = Predefiniti_Ricerca.SearchField;

            if (cmbSearchField.SelectedIndex < 0 && cmbSearchField.Items.Count > 0)
            {
                cmbSearchField.SelectedValue = ExamSearchField.DescrizioneEsame;
            }
        }

        private void LoadNavigation(ExamNavigationRequest request)
        {
            _isLoadingNavigation = true;
            try
            {
                var result = _navigationService.GetNavigation(request);

                BindLookupItems(lstRooms, NormalizeRoomItems(result.Rooms), result.SelectedRoomId);
                BindLookupItems(lstBodyParts, result.BodyParts, result.SelectedBodyPartId);
                BindExamItems(lstExams, result.Exams);
            }
            finally
            {
                _isLoadingNavigation = false;
            }
        }

        private static void BindLookupItems(ListBox listBox, IReadOnlyList<LookupItem> items, int? selectedId)
        {
            listBox.BeginUpdate();
            try
            {
                listBox.DataSource = null;
                listBox.DisplayMember = nameof(LookupItem.Label);
                listBox.ValueMember = nameof(LookupItem.Id);
                listBox.DataSource = new List<LookupItem>(items);

                if (selectedId.HasValue)
                {
                    listBox.SelectedValue = selectedId.Value;
                }

                if (listBox.SelectedIndex < 0 && listBox.Items.Count > 0)
                {
                    listBox.SelectedIndex = 0;
                }
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        private static void BindExamItems(ListBox listBox, IReadOnlyList<ExamListItem> items)
        {
            var displayItems = new List<ExamDisplayItem>();

            foreach (var exam in items)
            {
                displayItems.Add(new ExamDisplayItem
                {
                    Id = exam.Id,
                    DisplayLabel = BuildExamDisplayLabel(exam),
                    Exam = exam
                });
            }

            listBox.BeginUpdate();
            try
            {
                listBox.DataSource = null;
                listBox.DisplayMember = nameof(ExamDisplayItem.DisplayLabel);
                listBox.ValueMember = nameof(ExamDisplayItem.Id);
                listBox.DataSource = displayItems;

                if (listBox.SelectedIndex < 0 && listBox.Items.Count > 0)
                {
                    listBox.SelectedIndex = 0;
                }
            }
            finally
            {
                listBox.EndUpdate();
            }
        }

        private void LstExams_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            var baseFontHeight = Font.Height;
            e.ItemHeight = (baseFontHeight * 3) + 12;
        }

        private void LstExams_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index < 0 || e.Index >= lstExams.Items.Count)
            {
                return;
            }

            var displayItem = lstExams.Items[e.Index] as ExamDisplayItem;
            if (displayItem == null || displayItem.Exam == null)
            {
                e.DrawFocusRectangle();
                return;
            }

            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var foregroundColor = isSelected ? SystemColors.HighlightText : SystemColors.ControlText;
            var metaColor = isSelected ? SystemColors.HighlightText : SystemColors.HotTrack;

            var bounds = e.Bounds;
            var paddingLeft = bounds.Left + 6;
            var top = bounds.Top + 4;
            var availableWidth = Math.Max(0, bounds.Width - 12);

            using (var titleFont = new Font(e.Font, FontStyle.Bold))
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    displayItem.Exam.DescrizioneEsame ?? string.Empty,
                    titleFont,
                    new Rectangle(paddingLeft, top, availableWidth, e.Font.Height + 4),
                    foregroundColor,
                    TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
            }

            top += e.Font.Height + 4;

            TextRenderer.DrawText(
                e.Graphics,
                "Cod. Min.: " + (displayItem.Exam.CodiceMinisteriale ?? string.Empty),
                e.Font,
                new Rectangle(paddingLeft, top, availableWidth, e.Font.Height + 2),
                metaColor,
                TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);

            top += e.Font.Height + 2;

            TextRenderer.DrawText(
                e.Graphics,
                "Cod. Int.: " + (displayItem.Exam.CodiceInterno ?? string.Empty),
                e.Font,
                new Rectangle(paddingLeft, top, availableWidth, e.Font.Height + 2),
                metaColor,
                TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);

            e.DrawFocusRectangle();
        }

        private static List<LookupItem> NormalizeRoomItems(IReadOnlyList<LookupItem> items)
        {
            var normalizedItems = new List<LookupItem>();

            foreach (var item in items)
            {
                normalizedItems.Add(new LookupItem
                {
                    Id = item.Id,
                    Label = NormalizeRoomLabel(item.Label)
                });
            }

            return normalizedItems;
        }

        private static string NormalizeRoomLabel(string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                return string.Empty;
            }

            var normalized = Regex.Replace(label, @"(?<=[a-z])(?=[A-Z])", " ");
            normalized = Regex.Replace(normalized, @"(?<=[A-Za-z])(?=\d)", " ");

            return normalized.Trim();
        }

        private static string BuildExamDisplayLabel(ExamListItem exam)
        {
            if (exam == null)
            {
                return string.Empty;
            }

            return string.Format(
                "{0} | {1} | {2}",
                exam.DescrizioneEsame,
                exam.CodiceMinisteriale,
                exam.CodiceInterno);
        }

        private static List<SearchFieldOption> BuildSearchFieldOptions()
        {
            return new List<SearchFieldOption>
            {
                new SearchFieldOption
                {
                    Value = ExamSearchField.CodiceMinisteriale,
                    Label = "Codice ministeriale"
                },
                new SearchFieldOption
                {
                    Value = ExamSearchField.CodiceInterno,
                    Label = "Codice interno"
                },
                new SearchFieldOption
                {
                    Value = ExamSearchField.DescrizioneEsame,
                    Label = "Descrizione esame"
                }
            };
        }

        private static int? GetSelectedLookupId(ListBox listBox)
        {
            var selectedItem = listBox.SelectedItem as LookupItem;
            return selectedItem != null ? (int?)selectedItem.Id : null;
        }

        private ExamListItem GetSelectedExamItem()
        {
            var selectedItem = lstExams.SelectedItem as ExamDisplayItem;
            return selectedItem != null ? selectedItem.Exam : null;
        }

        private ExamSearchField GetSelectedSearchField()
        {
            var selectedItem = cmbSearchField.SelectedItem as SearchFieldOption;
            if (selectedItem != null)
            {
                return selectedItem.Value;
            }

            var selectedValue = cmbSearchField.SelectedValue;
            if (selectedValue is ExamSearchField)
            {
                return (ExamSearchField)selectedValue;
            }

            return ExamSearchField.DescrizioneEsame;
        }
    }
}
