import { useState, useEffect} from 'react';
import { BaseQuestion, BaseResponse } from './QuizQuestion'

export interface TextFieldQuestion extends BaseQuestion {
    fieldPlaceholder: string,
}

export interface TextFieldResponse extends BaseResponse {
    text: string,
}

const TextFieldQuestionComponent = (props: any) => {
    let defaultResponse: SelectResponse = {
        text: '',
        questionId: props.question.id,
    }
    const [questionProps, setQuestionProps] = useState<TextFieldQuestion>(props.question)
    const [response, setResponse] = useState<TextFieldResponse>(defaultResponse);

    function updateResponse(text: string) {
        let newResponse = { ...response, text: text };
        setResponse(newResponse);
        props.responseDictionary?.set(response.questionId, newResponse);
    }

    useEffect(
        () => {
            updateResponse(defaultResponse.text);
        }
        , []
    )

    return (
        <div className="text-field-question-wrapper">
            <label className="description">{questionProps.description}</label>
            <input value={response.text} onChange={(e) => updateResponse(e.target.value)} placeholder={questionProps.fieldPlaceholder}></input>
        </div>
    )
}

export default TextFieldQuestionComponent;