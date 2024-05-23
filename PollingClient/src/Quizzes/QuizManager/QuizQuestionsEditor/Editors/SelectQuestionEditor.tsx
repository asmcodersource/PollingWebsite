import { useState } from 'react';
import Button from 'react-bootstrap/Button';
import { putQuestionToServer } from './QuestionEditor'
import { SelectQuestion } from '../../../Questions/SelectQuestion';

const SelectQuestionEditor = (props) => {
    const [errorMsg, setErrorMsg] = useState<string>();
    const [question, setQuestion] = useState<SelectQuestion>(props.question);
    const [options, setOptions] = useState<string[]>(props.question?.options ?? []); 

    props.saveButtonSignal.handler = save;

    async function save() {
        let sendQuestion = { ...question }
        sendQuestion.options = options;
        let sendQuestionToServer = putQuestionToServer(sendQuestion, props.poll.id);
        sendQuestionToServer.then(
            () => {
                props.onHide();
                props.onUpdate();
            },
            async (response: Response) => {
                let responseBody = await response.json();
                console.log(responseBody);
                if (responseBody.hasOwnProperty('errors')) {
                    let errorMessage = ''
                    for (const error in responseBody['errors']) {
                        for (const msg of responseBody['errors'][error]) {
                            errorMessage = errorMessage + msg + ". ";
                        }
                    }
                    setErrorMsg(errorMessage);
                } else {
                    setErrorMsg("An unknown error has occured");
                }
            }
        )
    };

    function addOptionHandler() {
        let copy = [...options]
        copy.push("");
        setOptions(copy);
    }

    function removeOptionHandler() {
        if (options.length == 0)
            return;
        let copy = [...options]
        copy.pop()
        setOptions(copy);
    }

    function optionChanged(value: string, optionid: number) {
        let copy = [...options];
        copy[optionid] = value;
        setOptions(copy);
    }

    function questionPropertyChanged(newValue: any, propertyKey: keyof SelectQuestion) {
        let questionCopy = {...question}
        questionCopy[propertyKey] = newValue;
        setQuestion(questionCopy);
    }

    return (
        <div className="select-question-editor">
            <label>Field name</label><br />
            <input
                value={question.fieldName}
                id="field-name"
                placeholder="question name"
                onChange={(e) => questionPropertyChanged(e.target.value, "fieldName")}
            >
            </input><br />
            <label>Field description</label><br />
            <input
                value={question.description}
                id="field-description"
                placeholder="question description"
                onChange={(e) => questionPropertyChanged(e.target.value, "description")}
            >
            </input><br />
            <label>Options</label><br />
            {options.map((option, index) =>
                <input
                    className="select-option-input"
                    key={index}
                    value={option}
                    onChange={(e) => optionChanged(e.target.value, index)}
                    placeholder="option value">
                </input>
            )}
            <div className="buttons-group">
                <Button
                    variant="dark"
                    className="remove-option-button"
                    onClick={removeOptionHandler}
                    disabled={options.length == 0}
                >
                    Remove option
                </Button>
                <Button
                    variant="dark"
                    className="add-option-button"
                    onClick={addOptionHandler}
                >
                    Add option
                </Button>
                <p className="error">
                    {errorMsg}
                </p>
            </div>
        </div>

    )
}

export default SelectQuestionEditor;