
const SelectQuestionEditor = (props) => {
    return (
        <div className="select-question-editor">
            <label>Field name</label><br />
            <input
                id="field-name"
                placeholder="question name">
            </input><br />
            <label>Field description</label><br />
            <input
                id="field-description"
                placeholder="question description">
            </input><br />
            <label>Placeholder</label><br />
            <input
                id="field-placeholder"
                placeholder="placeholder of text field">
            </input><br />
        </div>
    )
}

export default SelectQuestionEditor;