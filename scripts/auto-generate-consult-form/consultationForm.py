import argparse
import os
from datetime import datetime
from docx import Document


def robust_replace_in_paragraph(paragraph, placeholder, replacement):
    count = 0
    runs = paragraph.runs
    if not runs:
        return 0

    full_text = "".join(run.text for run in runs)
    if placeholder not in full_text:
        return 0

    full_text = full_text.replace(placeholder, replacement)
    count = 1

    for i, run in enumerate(runs):
        run.text = ""
    runs[0].text = full_text

    return count


def replace_in_document(doc, placeholder, replacement):
    count = 0
    for p in doc.paragraphs:
        count += robust_replace_in_paragraph(p, placeholder, replacement)

    for table in doc.tables:
        for row in table.rows:
            for cell in row.cells:
                for p in cell.paragraphs:
                    count += robust_replace_in_paragraph(p, placeholder, replacement)

    for section in doc.sections:
        for p in section.header.paragraphs:
            count += robust_replace_in_paragraph(p, placeholder, replacement)
        for p in section.footer.paragraphs:
            count += robust_replace_in_paragraph(p, placeholder, replacement)

    return count


def main():
    script_dir = os.path.dirname(os.path.abspath(__file__))
    template_file = os.path.join(
        script_dir, "Accomplishment-and-Consultation-Form.docx"
    )

    parser = argparse.ArgumentParser(
        description="Replace placeholders in a .docx template and save."
    )
    # Updated help text to show it's a .docx file
    parser.add_argument("output", help="Output .docx file path")

    parser.add_argument(
        "--week", required=True, help="The week number to insert (e.g., '5')"
    )
    parser.add_argument(
        "--placeholder",
        default="{{DATE}}",
        help="Date placeholder text to replace (default: {{DATE}})",
    )
    parser.add_argument(
        "--week-placeholder",
        default="{{WEEK}}",
        help="Week number placeholder text to replace (default: {{WEEK}})",
    )
    parser.add_argument(
        "--date", help="Date string to insert (default: today's date in mm/dd/YYYY)"
    )
    args = parser.parse_args()

    try:
        doc = Document(template_file)
    except Exception as e:
        print(f"Error opening template {template_file}: {e}")
        return

    date_replacement = args.date or datetime.now().strftime("%m/%d/%Y")
    week_replacement = args.week

    count_date = replace_in_document(doc, args.placeholder, date_replacement)
    count_week = replace_in_document(doc, args.week_placeholder, week_replacement)

    print(
        f"Replacing {args.placeholder!r} -> {date_replacement!r} ({count_date} found)"
    )
    print(
        f"Replacing {args.week_placeholder!r} -> {week_replacement!r} ({count_week} found)"
    )

    try:
        # Simplified: Save the .docx file directly to the output path
        final_docx_path = os.path.abspath(args.output)
        doc.save(final_docx_path)
        print(f"\nSuccessfully generated DOCX: {final_docx_path}")

    except Exception as e:
        print(f"An unexpected error occurred while saving: {e}")


if __name__ == "__main__":
    main()
