using System;
using System.Collections.Generic;
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
            WireEvents();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitializeSearchField();
            LoadNavigation(new ExamNavigationRequest());
        }

        private void WireEvents()
        {
            lstRooms.SelectedIndexChanged += LstRooms_SelectedIndexChanged;
            lstBodyParts.SelectedIndexChanged += LstBodyParts_SelectedIndexChanged;
            btnConfirmExam.Click += BtnConfirmExam_Click;
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
            var selectedExam = lstExams.SelectedItem as ExamListItem;
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

        private void InitializeSearchField()
        {
            if (cmbSearchField.Items.Count > 0 && cmbSearchField.SelectedIndex < 0)
            {
                cmbSearchField.SelectedItem = ExamSearchField.DescrizioneEsame.ToString();
            }
        }

        private void LoadNavigation(ExamNavigationRequest request)
        {
            _isLoadingNavigation = true;
            try
            {
                var result = _navigationService.GetNavigation(request);

                BindLookupItems(lstRooms, result.Rooms, result.SelectedRoomId);
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
            listBox.BeginUpdate();
            try
            {
                listBox.DataSource = null;
                listBox.DisplayMember = nameof(ExamListItem.DescrizioneEsame);
                listBox.ValueMember = nameof(ExamListItem.Id);
                listBox.DataSource = new List<ExamListItem>(items);

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

        private static int? GetSelectedLookupId(ListBox listBox)
        {
            var selectedItem = listBox.SelectedItem as LookupItem;
            return selectedItem != null ? (int?)selectedItem.Id : null;
        }

        private ExamSearchField GetSelectedSearchField()
        {
            var selectedText = cmbSearchField.SelectedItem as string;
            ExamSearchField parsedValue;

            if (!string.IsNullOrWhiteSpace(selectedText)
                && Enum.TryParse(selectedText, out parsedValue))
            {
                return parsedValue;
            }

            return ExamSearchField.DescrizioneEsame;
        }
    }
}
