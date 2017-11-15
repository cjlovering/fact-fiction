import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ResultPane from '../components/ResultPane.jsx';

describe('ResultPane', () => {
    let textEntryTokens;
    let changeView;
    let selectEntry;
    let selectedEntryId;
    let resultPane;
    
    beforeEach(() => {
        changeView = jest.fn();
        selectEntry = jest.fn();
        selectedEntryId = "abc"; 
        textEntryTokens = [{
            "type": "OBJECTIVE",
            "id": "1",
            "content": "Hello world"
        }, {
            "type": "SUBJECTIVE",
            "id": "2",
            "content": "Hi"
        }];
        resultPane = mount(
            <ResultPane 
                changeView={changeView}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
                textEntryTokens={textEntryTokens} 
            />
        );
    });
    it('ResultPane requires changeView prop', () => {
        expect(resultPane.props().changeView).toBeDefined();
    });

    it('ResultPane requires textEntryTokens prop', () => {
        expect(resultPane.props().textEntryTokens).toBeDefined();
    });

    it('ResultPane requires selectEntry prop', () => {
        expect(resultPane.props().selectEntry).toBeDefined();
    });

    it('ResultPane requires selectedEntryId prop', () => {
        expect(resultPane.props().selectedEntryId).toBeDefined();
    });

    it('ResultPane has the right sentences', () => {
        expect(resultPane.props().textEntryTokens).toEqual(textEntryTokens);
    });
});