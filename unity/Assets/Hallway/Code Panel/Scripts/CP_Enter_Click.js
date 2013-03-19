#pragma strict

var correct_code = "5678";

function SetScriptsCondition(cond) {
	transform.parent.transform.Find("B0 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B1 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B2 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B3 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B4 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B5 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B6 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B7 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B8 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B9 Group").GetComponent.<CP_Digit_Click>().enabled = cond;
	transform.parent.transform.Find("B Reset Group").GetComponent.<CP_Reset_Click>().enabled = cond;
}

function OnMouseDown() {
	if (this.enabled) {
		var txt = transform.parent.transform.Find("Field Group").transform.Find("Field Text");
		var ind = transform.parent.transform.Find("Indicator");
		var l = txt.GetComponent(TextMesh).text.length;
		if (l == 0) {
			//���� ������������ �� ���� ������ ������, ������������� �� ������ ����� �������
			transform.Find("G Enter").animation.Play("CP ButtonError");
		} else {
			//���� ������������ ���� ���� ���-��, ��������� ��� �� ������������ ����������� ����
			//��������� ����������� �������� ������� ������
			transform.Find("G Enter").animation.Play("CP ButtonLight");
			//�������� ������� ���� ��������� ������, ����� ������������ �� ����� ��������
			SetScriptsCondition(false);
			if (txt.GetComponent(TextMesh).text == correct_code) {
				//����������� ��������� �����
				yield WaitForSeconds(0.4);
				//���������� ��������� � �������, ���������, ��� ��� ������
				ind.renderer.material.SetColor("_Color", Color(0.235, 0.235, 0.235));
				ind.renderer.material.SetColor("_SpecColor", Color(0, 1, 0));
				//����� ��������, ����� ������������ ����� ��������
				yield WaitForSeconds(1.5);
				//������������� ������ ������� � ���, ��� ���� ������������
				transform.parent.GetComponent.<CP_Begin_Click>().Finish();
			} else {
				//����������� ��������� �����
				//yield WaitForSeconds(0.4);
				//���������� ��������� � �������, ���������, ��� ��� ��������
				ind.renderer.material.SetColor("_Color", Color(0.235, 0.235, 0.235));
				ind.renderer.material.SetColor("_SpecColor", Color(1, 0, 0));
				//����� ��������, ����� ������������ ����� ��������
				yield WaitForSeconds(1.5);
				//������� ������ � ���������� ���������� �������� ����
				txt.GetComponent(TextMesh).text = "";
				ind.renderer.material.SetColor("_Color", Color(0, 0, 0));
				ind.renderer.material.SetColor("_SpecColor", Color(0.235, 0.235, 0.235));
			}
			//���������� ������� �� ���������� ���������
			SetScriptsCondition(true);
		}
	}	
}