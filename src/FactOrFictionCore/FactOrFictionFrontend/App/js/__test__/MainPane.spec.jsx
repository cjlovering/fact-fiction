import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import MainPane from '../components/MainPane.jsx';
import { VIEW_INPUT, VIEW_RESULT } from '../constants/viewTypes';

describe('MainPane', () => {

    let mainPane;
    let mainPaneResult;
    let changeView;
    let textEntryTokens;
    let fetchTextEntry;
    let selectEntry;
    let selectedEntryId;
    
    beforeEach(() => {
        fetchTextEntry = jest.fn();
        changeView = jest.fn();
        selectEntry = jest.fn();
        selectedEntryId = "abc"; 
        textEntryTokens = [{
            "type": "OBJECTIVE",
            "id": "123",
            "sentence": "Hello"
        }];
        mainPane = mount(
            <MainPane 
                fetchTextEntry={fetchTextEntry} 
                changeView={changeView}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
                textEntryTokens={textEntryTokens} 
                view={VIEW_INPUT} 
            />
        );
        mainPaneResult = mount(
            <MainPane 
                fetchTextEntry={fetchTextEntry} 
                changeView={changeView} 
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
                textEntryTokens={textEntryTokens} 
                view={VIEW_RESULT} 
            />
        );
    });

    it('MainPane requires fetchTextEntry prop', () => {
        expect(mainPane.props().fetchTextEntry).toBeDefined();
    });

    it('MainPane requires changeView prop', () => {
        expect(mainPane.props().changeView).toBeDefined();
    });

    it('MainPane requires view prop', () => {
        expect(mainPane.props().view).toBeDefined();
    });

    it('MainPane requires textEntryTokens prop', () => {
        expect(mainPane.props().textEntryTokens).toBeDefined();
    });

    it('MainPane requires selectEntry prop', () => {
        expect(mainPane.props().selectEntry).toBeDefined();
    });

    it('MainPane requires selectedEntryId prop', () => {
        expect(mainPane.props().selectedEntryId).toBeDefined();
    });

    it('App renders nested components (input)', () => {
        expect(mainPane.find('InputPane').length).toEqual(1);
    });

    it('App renders nested components (result)', () => {
        expect(mainPaneResult.find('ListView').length).toEqual(1);
    });

    it('Button click calls changeView', () => {
        let button = mainPaneResult.find('.change-view-button').first();
        button.simulate('click');
        expect(changeView).toBeCalledWith(VIEW_INPUT);
    });
});